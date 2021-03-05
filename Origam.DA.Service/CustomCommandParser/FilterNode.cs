﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Origam.Schema;
using Origam.Schema.EntityModel;

namespace Origam.DA.Service.CustomCommandParser
{
    class FilterNode
    {
        private readonly string nameLeftBracket;
        private readonly string nameRightBracket;
        private readonly Dictionary<string, string> lookupExpressions;
        private readonly List<ColumnInfo> columns;
        private string[] splitValue;
        public FilterNode Parent { get; set; }
        public List<FilterNode> Children { get; } = new List<FilterNode>();
        public string Value { get; set; } = "";
        public bool IsBinaryOperator => Value.Contains("$");

        public IEnumerable<FilterNode> AllChildren
        {
            get
            {
                yield return this;
                foreach (var child in Children)
                {
                    foreach (var child1 in child.AllChildren)
                    {
                        yield return child1;
                    }
                }
            }
        }
        
        private bool ContainsNumbersOnly(string value)
        {
            return Regex.Match(value, "^[\\d,\\s]+$").Success;
        }

        private string[] SplitValue
        {
            get
            {
                if (splitValue == null)
                {
                    splitValue = ContainsNumbersOnly(Value) 
                        ? Value.Split(',') 
                        : Regex.Split(Value, "[\\]\"]\\s*,\\s*[\\[\"]?");
                    splitValue = splitValue
                        .Select(x => x.Trim())
                        .ToArray();
                }

                return splitValue;
            }
        }

        internal string ColumnName => SplitValue[0].Replace("\"", "");

        private string RenderedColumnName =>
            lookupExpressions.ContainsKey(ColumnName)
                ? lookupExpressions[ColumnName]
                : nameLeftBracket + ColumnName + nameRightBracket;


        private string Operator => SplitValue?.Length > 1 
            ? SplitValue[1].Replace("\"","") 
            : null;
        private string ColumnValue => ValueToOperand(SplitValue[2]);

        private ColumnInfo Column =>
            columns
                .FirstOrDefault(column => column.Name == ColumnName)
            ?? throw new Exception($"Unknown column \"{ColumnName}\"");

        public OrigamDataType ParameterDataType {
            get
            {
                if (Column.DataType == OrigamDataType.Array)
                {
                    return OrigamDataType.String;
                }

                if (Column.DataType == OrigamDataType.UniqueIdentifier &&
                    Operator != "eq" && Operator != "neq" && Operator != "in" 
                    && Operator != "nin")
                {
                    return OrigamDataType.String;
                }

                return Column.DataType;
            }
        }

        public bool IsValueNode => Parent != null && 
                                   (Parent.Operator == "in" ||
                                    Parent.Operator == "nin" ||
                                    Parent.Operator == "between" ||
                                    Parent.Operator == "nbetween");

        private readonly AbstractFilterRenderer renderer;
        private readonly List<ParameterData> parameterDataList;
        private readonly string parameterReferenceChar;

        public FilterNode(string nameLeftBracket, string nameRightBracket, Dictionary<string,string> lookupExpressions, 
            List<ColumnInfo> columns,
            AbstractFilterRenderer filterRenderer, List<ParameterData> parameterDataList,
            string parameterReferenceChar)
        {
            this.nameLeftBracket = nameLeftBracket;
            this.nameRightBracket = nameRightBracket;
            this.lookupExpressions = lookupExpressions;
            this.columns = columns;
            renderer = filterRenderer;
            this.parameterDataList = parameterDataList;
            this.parameterReferenceChar = parameterReferenceChar;
        }

        private string GetParameterName(string columnName)
        {
            return parameterReferenceChar + columnName;
        }

        private string ValueToOperand(string value)
        {
            return value.Replace("\"", "");
        }

        private object ToDbValue(string value, OrigamDataType dataType)
        {
            switch(dataType)
            {
                case OrigamDataType.Integer:
                    if (!int.TryParse(value, out var intValue))
                    {
                        throw new ArgumentOutOfRangeException($"Cannot parse \"{value}\" to int");
                    }
                    return intValue;
                case OrigamDataType.Currency:
                    if (!decimal.TryParse(value, out var decimalValue))
                    {
                        throw new ArgumentOutOfRangeException($"Cannot parse \"{value}\" to decimal");
                    }
                    return decimalValue;
                case OrigamDataType.Float:
                    if (!float.TryParse(value, out var floatValue))
                    {
                        throw new ArgumentOutOfRangeException($"Cannot parse \"{value}\" to float");
                    }
                    return floatValue;
                case OrigamDataType.Boolean:
                    if (!bool.TryParse(value, out var boolValue))
                    {
                        throw new ArgumentOutOfRangeException($"Cannot parse \"{value}\" to bool");
                    }
                    return boolValue;
                case OrigamDataType.Date:
                    if (string.IsNullOrEmpty(Convert.ToString(value)))
                    {
                        return  null;
                    }
                    if (!DateTime.TryParse(value, out var dateValue))
                    {
                        throw new ArgumentOutOfRangeException($"Cannot parse \"{value}\" to DateTime");
                    }
                    return dateValue;
                case OrigamDataType.UniqueIdentifier:
                    if(value == null)
                    {
                        return Guid.Empty;
                    }
                    if (!Guid.TryParse(value, out Guid guidValue))
                    {
                        throw new ArgumentOutOfRangeException($"Cannot parse \"{value}\" to Guid");
                    }
                    return guidValue;
            }
            return value;
        }
        
        public string SqlRepresentation()
        {
            if (IsBinaryOperator)
            {
                return GetSqlOfOperatorNode();
            }
            else
            {
                return GetSqlOfLeafNode();
            }
        }

        private string GetSqlOfOperatorNode()
        {
            string logicalOperator = GetLogicalOperator();
            List<string> operands = Children
                .Select(node => node.SqlRepresentation()).ToList();
            return renderer.LogicalAndOr(logicalOperator, operands);
        }

        private string GetLogicalOperator()
        {
            if (Value.Trim() == "\"$AND\"") return "AND";
            if (Value.Trim() == "\"$OR\"") return "OR";
            throw new Exception("Could not parse node value to logical operator: \"" + Value+"\"");
        }

        private string GetSqlOfLeafNode()
        {
            string nodeSql = RenderNodeSql();
            if (Operator.StartsWith("n") && Column.IsNullable)
            {
                string isNullSql = renderer.BinaryOperator(
                    leftValue: RenderedColumnName,
                    operatorName: "Equal",
                    rightValue: null);
                return renderer.LogicalAndOr("OR", new []{nodeSql, isNullSql});
            }
            return nodeSql;
        }

        private string RenderNodeSql()
        {
            string parameterName = GetParameterName(ColumnName);
            var (operatorName, renderedColumnValue) =
                GetRendererInput(Operator, parameterName);
            if (Children.Count == 0)
            {
                if (SplitValue.Length != 3)
                {
                    throw new ArgumentException("could not parse: " + Value +
                                                " to a filter node");
                }

                if (ColumnValue == null || ColumnValue.ToLower() == "null")
                {
                    return renderer.BinaryOperator(
                        leftValue: RenderedColumnName,
                        rightValue: null,
                        operatorName: operatorName);
                }

                object value = ToDbValue(ColumnValue, Column.DataType);
                if ((Operator == "eq" || Operator == "neq") &&
                    Column.DataType == OrigamDataType.Date &&
                    IsWholeDay((DateTime)value))
                {
                    return RenderDateEquals();
                }

                parameterDataList.Add(new ParameterData
                (
                    columnName: ColumnName,
                    parameterName: ColumnName,
                    value: value,
                    dataType: ParameterDataType
                ));
                return renderer.BinaryOperator(
                    leftValue: RenderedColumnName,
                    rightValue: renderedColumnValue,
                    operatorName: operatorName);
            }

            if (Children.Count == 1 &&
                (Operator == "in" || Operator == "nin" || Operator == "between" ||
                 Operator == "nbetween"))
            {
                var parameterNames = GetRightHandValues()
                    .Select((value, i) =>
                    {
                        string columnNameNumbered = ColumnName + "_" + i;
                        parameterDataList.Add(new ParameterData
                            (
                                columnName: ColumnName,
                                parameterName: columnNameNumbered,
                                value: value,
                                dataType: ParameterDataType
                            )
                        );
                        return GetParameterName(columnNameNumbered);
                    })
                    .ToArray();
                return renderer.BinaryOperator(
                    columnName: ColumnName,
                    leftValue: RenderedColumnName,
                    rightValues: parameterNames,
                    operatorName: operatorName,
                    isColumnArray: Column.DataType == OrigamDataType.Array);
            }

            throw new Exception("Cannot parse filter node: " + Value +
                                ". If this should be a binary operator prefix it with \"$\".");
        }

        private object[] GetRightHandValues()
        {
            object[] rightHandValues = Children.First()
                .SplitValue
                .Select(ValueToOperand)
                .Select(value => ToDbValue(value, Column.DataType))
                .ToArray();
            if (Column.DataType == OrigamDataType.Date &&
                rightHandValues.Length == 2 &&
                IsWholeDay((DateTime) rightHandValues[1]))
            {
                rightHandValues[1] = ((DateTime) rightHandValues[1])
                    .AddDays(1).AddSeconds(-1);
            }

            return rightHandValues;
        }

        private bool IsWholeDay(DateTime dateTime)
        {
            return dateTime.Millisecond == 0 && dateTime.Second == 0 &&
                   dateTime.Minute == 0 && dateTime.Hour == 0;
        }

        private string RenderDateEquals()
        {
            string actualOperator;
            switch (Operator)
            {
                case "eq":
                    actualOperator = "between";
                    break;
                case "neq":
                    actualOperator = "nbetween";
                    break;
                default:
                    throw new InvalidOperationException("Operator must be eq or neq");
            }
            
            var (operatorName, _) =
                GetRendererInput(actualOperator, ""); 

            DateTime equalsDate = (DateTime)ToDbValue(ColumnValue, Column.DataType);
            DateTime startDate = new DateTime(equalsDate.Year, equalsDate.Month, equalsDate.Day);
            DateTime endDate = startDate.AddDays(1).AddSeconds(-1);
            var parameterNames = new[] { startDate, endDate }
                .Select((value, i) =>
                {
                    string columnNameNumbered = ColumnName + "_" + i;
                    parameterDataList.Add(new ParameterData
                    (
                        columnName: ColumnName,
                        parameterName: columnNameNumbered,
                        value: value,
                        dataType: ParameterDataType
                    ));
                    return GetParameterName(columnNameNumbered);
                })
                .ToArray();

            return renderer.BinaryOperator(
                columnName: ColumnName,
                leftValue: RenderedColumnName, 
                rightValues: parameterNames, 
                operatorName: operatorName,
                isColumnArray: Column.DataType == OrigamDataType.Array);
        }

        private (string,string) GetRendererInput(string operatorName, string value)
        {
            switch (operatorName)
            {
                case "gt": return ("GreaterThan", value);
                case "lt": return ("LessThan", value);
                case "gte": return ("GreaterThanOrEqual", value);
                case "lte": return ("LessThanOrEqual", value);
                case "eq": return ("Equal", value);
                case "neq": return ("NotEqual", value);
                case "starts": return ("Like", AppendWildCard(value));
                case "nstarts": return ("NotLike", AppendWildCard(value));
                case "ends": return ("Like", PrependWildCard(value));
                case "nends": return ("NotLike",  PrependWildCard(value));
                case "like":
                case "contains": return ("Like", PrependWildCard(AppendWildCard(value)));
                case "ncontains": return ("NotLike", PrependWildCard(AppendWildCard(value)));
                case "null": return ("Equal", null);
                case "nnull": return ("NotEqual", null);
                case "between": return ("Between", null);
                case "nbetween": return ("NotBetween", null);
                case "in": return ("In", null);
                case "nin": return ("NotIn", null);
                default: throw new NotImplementedException(operatorName);
            }
        }

        private string PrependWildCard(string value)
        {
            if (!value.StartsWith(parameterReferenceChar))
            {
                throw new ArgumentException("Cannot prepend \"%\" to a value which is not a parameter");
            }
            return "'%' " + renderer.StringConcatenationChar+" "+value;
        } 
        private string AppendWildCard(string value)
        {
            if (!value.StartsWith(parameterReferenceChar))
            {
                throw new ArgumentException("Cannot append \"%\" to a value which is not a parameter");
            }
            // return value + "+'%'";
            return value +" "+ renderer.StringConcatenationChar + " '%'";
        }
    }
}

