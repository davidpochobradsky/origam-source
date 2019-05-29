using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Origam.DA.ObjectPersistence;
using Origam.Schema.WorkflowModel;
using Origam.Workbench.Diagram.NodeDrawing;
using DrawingNode = Microsoft.Msagl.Drawing.Node;

namespace Origam.Workbench.Diagram.DiagramFactory
{
    class ContextStoreDiagramFactory: IDiagramFactory<IContextStore, Graph>
    {

        private Graph graph;
        private readonly IPersistenceProvider persistenceProvider;
        private readonly NodeFactory nodeFactory;

        public ContextStoreDiagramFactory(
            IPersistenceProvider persistenceProvider,
            INodeSelector nodeSelector, GViewer gViewer)
        {
            this.persistenceProvider = persistenceProvider;
            nodeFactory = new NodeFactory(nodeSelector, gViewer);
        }

        public Graph Draw(IContextStore contextStore)
        {
            graph = new Graph();
            
            Node storeNode = nodeFactory.AddNode(graph, contextStore);
            List<IWorkflowStep> steps = persistenceProvider
                .RetrieveList<IWorkflowStep>();
            
            foreach (IWorkflowStep step in steps)
            {
                if (step is WorkflowTask task &&
                    task.OutputContextStoreId == contextStore.Id)
                {
                    Node taskNode = nodeFactory.AddNode(graph, task);
                    graph.AddEdge(storeNode.Id, taskNode.Id);
                }
                else if (step is UpdateContextTask updateTask &&
                          updateTask.XPathContextStore.Id == contextStore.Id)
                {
                    Node taskNode = nodeFactory.AddNode(graph, updateTask);
                    graph.AddEdge(taskNode.Id, storeNode.Id);
                }
            }

            return graph;
        }
    }
}