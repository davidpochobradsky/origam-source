#region license
/*
Copyright 2005 - 2020 Advantage Solutions, s. r. o.

This file is part of ORIGAM (http://www.origam.org).

ORIGAM is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

ORIGAM is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with ORIGAM. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Data;
using System.Xml;
using System.Collections;
using Origam.Schema;
using Origam.Schema.WorkflowModel;
using System.Collections.Generic;

namespace Origam.Workflow
{
	/// <summary>
	/// Summary description for WorkflowServiceAgent.
	/// </summary>
	public class WorkflowServiceAgent : AbstractServiceAgent
	{
		public WorkflowServiceAgent()
		{
		}

		#region Private Methods
		private object ExecuteWorkflow(Guid workflowId, Hashtable parameters)
		{
			bool invalidWorkflowDefinition = false;

			IWorkflow wf = null;

			try
			{
				wf = this.PersistenceProvider.RetrieveInstance(typeof(AbstractSchemaItem), new ModelElementKey(workflowId)) as IWorkflow;
			}
			catch
			{
				invalidWorkflowDefinition = true;
			}

			if(wf == null || invalidWorkflowDefinition)
			{
				throw new ArgumentOutOfRangeException("workflowId", workflowId, ResourceUtils.GetString("ErrorWorkflowDefinition"));
			}

			WorkflowEngine engine = new WorkflowEngine();
			engine.PersistenceProvider = this.PersistenceProvider;
			engine.WorkflowBlock = wf;
            engine.TransactionBehavior = wf.TransactionBehavior;
			engine.TransactionId = this.TransactionId;
			engine.WorkflowInstanceId = this.TraceWorkflowId;
			engine.CallingWorkflow = this.WorkflowEngine as WorkflowEngine;
		    engine.Name = string.IsNullOrEmpty(wf.Name) ?
		        "WorkFlow" : wf.Name;
			engine.Trace = this.Trace;

            // input parameters
            foreach (DictionaryEntry entry in parameters)
			{
				AbstractSchemaItem context = wf.GetChildByName((string)entry.Key, ContextStore.CategoryConst);
					
				if(context == null)
				{
					throw new ArgumentOutOfRangeException("name", entry.Key, ResourceUtils.GetString("ErrorWorkflowContext", ((AbstractSchemaItem)wf).Path));
				}

				object contextValue = entry.Value;
				if(contextValue is DataSet)
				{
					contextValue = DataDocumentFactory.New(contextValue as DataSet);
				}
					
				engine.InputContexts.Add(context.PrimaryKey, contextValue);
			}

			WorkflowHost host;

            if (this.WorkflowEngine != null)
            {
                host = (this.WorkflowEngine as WorkflowEngine).Host;
            }
            else
            {
                host = WorkflowHost.DefaultHost;
            }

            IAsyncResult thread = host.ExecuteWorkflow(engine);

			thread.AsyncWaitHandle.WaitOne();

			if(engine.Exception != null)
			{
				throw engine.Exception;
			}

			return engine.ReturnValue;
		}
		#endregion

		#region IServiceAgent Members
		private object _result;
		public override object Result
		{
			get
			{
				return _result;
			}
		}

		public override void Run()
		{
			switch(this.MethodName)
			{
				case "ExecuteWorkflow":
					// Check input parameters
					if(! (this.Parameters["Workflow"] is Guid))
						throw new InvalidCastException(ResourceUtils.GetString("ErrorWorkflowNotGuid"));
					
					if(! (this.Parameters["Parameters"] is Hashtable))
						throw new InvalidCastException(ResourceUtils.GetString("ErrorNotHashtable"));

					_result = this.ExecuteWorkflow((Guid)this.Parameters["Workflow"],
						(Hashtable)this.Parameters["Parameters"]);

					break;

				default:
					throw new ArgumentOutOfRangeException("MethodName", this.MethodName, ResourceUtils.GetString("InvalidMethodName"));
			}
		}

		public override IList<string> ExpectedParameterNames(AbstractSchemaItem item, string method, string parameter)
		{
			var result = new List<string>();
			IWorkflow wf = item as IWorkflow;
			ServiceMethodCallTask task = item as ServiceMethodCallTask;
			if(task != null)
			{
				wf = ResolveServiceMethodCallTask(task);
			}
			if(wf != null && method == "ExecuteWorkflow" && parameter == "Parameters")
			{
				foreach(ContextStore cs in wf.ChildItemsByType(ContextStore.CategoryConst))
				{
					result.Add(cs.Name);
				}
			}

			return result;
		}

		private IWorkflow ResolveServiceMethodCallTask(ServiceMethodCallTask task)
		{
			AbstractSchemaItem wfParam = task.GetChildByName("Workflow");
			if(wfParam.ChildItems.Count == 1)
			{
				WorkflowReference wfRef = wfParam.ChildItems[0] as WorkflowReference;
				if(wfRef != null)
				{
					return wfRef.Workflow;
				}
			}
			return null;
		}
		#endregion
	}
}
