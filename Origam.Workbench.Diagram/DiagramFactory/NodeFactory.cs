using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Origam.Schema;
using Origam.Workbench.Diagram.Extensions;
using Node = Microsoft.Msagl.Drawing.Node;
using Point = Microsoft.Msagl.Core.Geometry.Point;

namespace Origam.Workbench.Diagram.DiagramFactory
{
    class NodeFactory
    {
        private static readonly int margin = 3;
        private static readonly int textSideMargin = 15;
        private static readonly Font font = new Font("Arial", 10);
        private static readonly SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);
        private static readonly StringFormat drawFormat = new StringFormat();
        private static readonly Graphics measurementGraphics = new Control().CreateGraphics();
        private static readonly Pen boldBlackPen = new Pen(System.Drawing.Color.Black, 2);
        private static readonly Pen blackPen =new Pen(System.Drawing.Color.Black, 1);
        private static readonly SolidBrush greyBrush = new SolidBrush(System.Drawing.Color.LightGray);
        private static readonly int nodeHeight = 25;
        private readonly INodeSelector nodeSelector;

        public NodeFactory(INodeSelector nodeSelector)
        {
            this.nodeSelector = nodeSelector;
        }

        public Node AddNode(Graph graph, ISchemaItem schemaItem)
        {
            Node node = graph.AddNode(schemaItem.Id.ToString());
            node.Attr.Shape = Shape.DrawFromGeometry;
            node.DrawNodeDelegate = DrawNode;
            node.NodeBoundaryDelegate = GetNodeBoundary;
            node.UserData = schemaItem;
            node.LabelText = schemaItem.Name;
            return node;
        }
        
        public Subgraph AddSubgraphNode(Subgraph parentSbubgraph, ISchemaItem schemaItem)
        {
            Subgraph subgraph = new Subgraph(schemaItem.Id.ToString());
            subgraph.Attr.Shape = Shape.DrawFromGeometry;
            subgraph.DrawNodeDelegate = DrawSubgraph;
            subgraph.NodeBoundaryDelegate = GetSubgraphBoundary;
            subgraph.UserData = schemaItem;
            subgraph.LabelText = schemaItem.Name;
            parentSbubgraph.AddSubgraph(subgraph);
            return subgraph;
        }

        private ICurve GetSubgraphBoundary(Node node)
        {
            return GetNodeBoundary(node);
        }

        private bool DrawSubgraph(Node node, object graphics)
        {
            return DrawNode(node, graphics);
        }

        private ICurve GetNodeBoundary(Node node) {
            var borderSize = CalculateBorderSize(node);
            return CurveFactory.CreateRectangle(borderSize.Width, borderSize.Height, new Point());
        }

        private bool DrawNode(Node node, object graphicsObj) {
            Graphics editorGraphics = (Graphics)graphicsObj;
            var image = GetImage(node);

            Pen pen = nodeSelector.Selected == node
                ? boldBlackPen 
                : blackPen;
            
            SizeF stringSize = editorGraphics.MeasureString(node.LabelText, font);

            var borderSize = CalculateBorderSize(node);
            var borderCorner = new System.Drawing.Point(
                (int)node.GeometryNode.Center.X - borderSize.Width / 2,
                (int)node.GeometryNode.Center.Y - borderSize.Height / 2);
            Rectangle border = new Rectangle(borderCorner, borderSize);
            Rectangle imageBackground = new Rectangle(borderCorner, new Size(nodeHeight, nodeHeight));

            var labelPoint = new PointF(
                (float)node.GeometryNode.Center.X - (float)border.Width / 2 + nodeHeight + margin  + textSideMargin,
                (float)node.GeometryNode.Center.Y - (int)stringSize.Height / 2);

            var imageHorizontalBorder = (imageBackground.Width - image.Width) / 2;
            var imageVerticalBorder = (imageBackground.Height - image.Height) / 2;
            var imagePoint = new PointF(
                (float)(node.GeometryNode.Center.X - (float)border.Width / 2 + imageHorizontalBorder),
                (float)(node.GeometryNode.Center.Y - (float)border.Height / 2 + imageVerticalBorder));

            editorGraphics.DrawUpSideDown(drawAction: graphics =>
                {                   
                    graphics.DrawString(node.LabelText, font, drawBrush, labelPoint, drawFormat);
                    graphics.FillRectangle(greyBrush, imageBackground);
                    graphics.DrawImage(image, imagePoint);
                    graphics.DrawRectangle(pen, border);
                }, 
                yAxisCoordinate: (float)node.GeometryNode.Center.Y);
            
            return true;
        }
        
        private static Image GetImage(Node node)
        {
            var schemaItem = (ISchemaItem) node.UserData;

            var schemaBrowser =
                WorkbenchSingleton.Workbench.GetPad(typeof(IBrowserPad)) as IBrowserPad;
            var imageList = schemaBrowser.ImageList;
            Image image = imageList.Images[schemaBrowser.ImageIndex(schemaItem.Icon)];
            return image;
        }

        private Size CalculateBorderSize(Node node)
        {
            SizeF stringSize = measurementGraphics.MeasureString(node.LabelText, font);

            int totalWidth = (int) (margin + nodeHeight + textSideMargin + stringSize.Width + textSideMargin);
            return  new Size(totalWidth, nodeHeight);
        }
    }
}