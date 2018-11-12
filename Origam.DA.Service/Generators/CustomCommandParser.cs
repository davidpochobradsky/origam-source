﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ArgumentException = System.ArgumentException;

namespace Origam.DA.Service.Generators
{
    public class CustomCommandParser
    {
        private Node root = null;
        private Node currentNode = null;

        /// <summary>
        /// returns ORDER BY clause without the "ORDER BY" keyword
        /// </summary>
        /// <param name="ordering"> [[columnName, "asc"|"desc"],...] </param>
        /// <returns></returns>
        public string ToSqlOrderBy(List<Tuple<string,string>> ordering)
        {
            if (ordering == null) throw new ArgumentException(nameof(ordering)+ " cannot be null");
            return string.Join(", ",
                ordering
                    .Select(x => new ColumnOrdering(x.Item1, x.Item2))
                    .Select(x => x.ToString())
                );
        }

        /// <summary>
        /// returns WHERE clause without the "WHERE" keyword
        /// </summary>
        /// <param name="strFilter">input example: "[\"$AND\", [\"$OR\",[\"city_name\",\"like\",\"%Wash%\"],[\"name\",\"like\",\"%Smith%\"]], [\"age\",\"gte\",18],[\"id\",\"in\",[\"f2\",\"f3\",\"f4\"]]";
        /// </param>
        public string ToSqlWhere(string strFilter)
        {
            if (strFilter?.Trim() == "") return "";
            var inpValue = GetCheckedInput(strFilter);
            ParseToNodeTree(inpValue);
            return root.SqlRepresentation();
        }

        private void ParseToNodeTree(string inpValue)
        {
            root = null;
            currentNode = null;
            foreach (char c in inpValue)
            {
                if (c == '[')
                {
                    AddNode();
                }
                else if (c == ']')
                {
                    currentNode = currentNode.Parent;
                }
                else if (c == ' ' || currentNode.IsBinaryOperator && c == ',')
                {
                    continue;
                }
                else
                {
                    currentNode.Value += c;
                }
            }
        }

        private void AddNode()
        {
            Node newNode = new Node();
            newNode.Parent = currentNode;
            currentNode?.Children.Add(newNode);
            currentNode = newNode;
            if (root == null)
            {
                root = newNode;
            }
        }

        private static string GetCheckedInput(string strFilter)
        {
            if (strFilter == null)
            {
                throw new ArgumentException(nameof(strFilter) + " cannot be null");
            }

            string inpValue = strFilter.Trim();
            if (inpValue[0] != '[')
            {
                throw new ArgumentException("Filter input must start with \"[\", found: \"" + inpValue[0] + "\"");
            }

            if (inpValue.Last() != ']')
            {
                throw new ArgumentException("Filter input must end with \"]\", found: \"" + inpValue.Last() + "\"");
            }

            if (inpValue.Last() != ']')
            {
                throw new ArgumentException("Filter input must end with \"]\", found: \"" + inpValue.Last() + "\"");
            }

            if (inpValue.Count(x => x == ']') != inpValue.Count(x => x == ']'))
            {
                throw new ArgumentException("Filter input must contain the same number of \"[\" and \"]\", input is: \"" + inpValue + "\"");
            }

            return inpValue;
        }
    }

    class ColumnOrdering
    {
        public string Column { get; }
        public string OrderingSql { get; }

        public ColumnOrdering(string column, string orderingName)
        {
            if (string.IsNullOrWhiteSpace(column))
            {
                throw new ArgumentException(nameof(column) + " cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(orderingName))
            {
                throw new ArgumentException(nameof(orderingName) + " cannot be empty");
            }
            Column = column;
            OrderingSql = OrderingToSQLName(orderingName);
        }

        private string OrderingToSQLName(string orderingName)
        {
            switch (orderingName)
            {
                case "asc": return "ASC";
                case "desc": return "DESC";
                default: throw new NotImplementedException(orderingName);
            }
        }

        public override string ToString()
        {
            return Column + " " + OrderingSql;
        }
    }

    class Node
    {
        private string[] splitValue;
        public Node Parent { get; set; }
        public List<Node> Children { get; } = new List<Node>();
        public string Value { get; set; } = "";
        public bool IsBinaryOperator => Value.Contains("$");
        public string[] SplitValue => splitValue ?? (splitValue = Value.Split(','));
        private string LeftOperand => SplitValue[0].Replace("\"","");
        private string Operator => SplitValue[1].Replace("\"","");
        private string RightOperand => SplitValue[2].Replace("\"", "'");
        private readonly FilterRenderer renderer = new FilterRenderer();

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
            return Children
                .Select(node => node.SqlRepresentation())
                .Select(operand => "("+ operand + ")")
                .Aggregate((x, y) => x + " " + logicalOperator + " " + y);
        }

        private string GetLogicalOperator()
        {
            if (Value == "\"$AND\"") return "AND";
            if (Value == "\"$OR\"") return "OR";
            throw new Exception("Could not parse node value to logical operator: " + Value);
        }

        private string GetSqlOfLeafNode()
        {
            if (Children.Count == 0)
            {
                if (SplitValue.Length != 3) throw new ArgumentException("could not parse: "+Value+" to a filter node");
                string operatorName = OperatorToRendererName(Operator);
                return renderer.BinaryOperator(LeftOperand, RightOperand, operatorName);
            }

            if (Children.Count == 1 && Operator == "in")
            {
                string options = Children.First()
                    .SplitValue
                    .Select(val => val.Replace("\"","'"))
                    .Aggregate((x,y) => x+", "+y);
                return LeftOperand + " IN (" + options + ")";
            }

            throw new Exception("Cannot parse filter node: " + Value);
        }

        private string OperatorToRendererName(string operatorName)
        {
            switch (operatorName)
            {
                case "gt": return "GreaterThan";
                case "lt": return "LessThan";
                case "gte": return "GreaterThanOrEqual";
                case "lte": return "LessThanOrEqual";
                case "eq": return "Equal";
                case "neq": return "NotEqual";
                case "like": return "Like";
                default: throw new NotImplementedException(operatorName);
            }
        }
    }
}