using System.Drawing;
using System.Linq;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using Origam.Workbench.Diagram.Extensions;
using Node = Microsoft.Msagl.Drawing.Node;
using Point = Microsoft.Msagl.Core.Geometry.Point;

namespace Origam.Workbench.Diagram.NodeDrawing
{
    internal class SubgraphNodePainter : INodeItemPainter
    {
        private readonly InternalPainter painter;
        private readonly NodePainter nodePainter;
        private readonly NodeHeaderPainter nodeHeaderPainter;

        public SubgraphNodePainter(InternalPainter internalPainter,
            bool isFromActivePackage)
        {
            painter = internalPainter;
            nodePainter = new NodePainter(internalPainter, isFromActivePackage);
            nodeHeaderPainter = new NodeHeaderPainter(internalPainter, isFromActivePackage);
        }
        
        public ICurve GetBoundary(Node node)
        {
            Subgraph subgraph = (Subgraph) node;
            if (!subgraph.Nodes.Any() && ! subgraph.Subgraphs.Any())
            {
                return nodePainter.GetBoundary(node);
            }

            var clusterBoundary =
                ((Cluster) node.GeometryNode).RectangularBoundary;

            var height = clusterBoundary.TopMargin;
            var labelWidth = GetLabelWidth(node);

            var width = clusterBoundary.MinWidth > labelWidth
                ? clusterBoundary.MinWidth
                : labelWidth + painter.LabelSideMargin * 2;

            return CurveFactory.CreateRectangle(width, height, new Point());
        }

        public bool Draw(Node node, object graphicsObj)
        {
            Subgraph subgraph = (Subgraph) node;
            if (!subgraph.Nodes.Any())
            {
                return nodePainter.Draw(node, graphicsObj);
            }

            var borderSize = new Size(
                (int) node.BoundingBox.Width,
                (int) node.BoundingBox.Height);
            var borderCorner = new System.Drawing.Point(
                (int) node.GeometryNode.Center.X - borderSize.Width / 2,
                (int) node.GeometryNode.Center.Y - borderSize.Height / 2);
            Rectangle border = new Rectangle(borderCorner, borderSize);

            Rectangle headerBorder = new Rectangle(
                borderCorner.X, 
                border.Bottom - painter.NodeHeaderHeight, 
                border.Width, 
                painter.NodeHeaderHeight);
            
            Graphics editorGraphics = (Graphics) graphicsObj;
            nodeHeaderPainter.Draw(node, editorGraphics, headerBorder);
            
            editorGraphics.DrawUpSideDown(drawAction: graphics =>
            {
                graphics.DrawRectangle(painter.GetActiveBorderPen(node), border);

            },
            yAxisCoordinate: (float) node.GeometryNode.Center.Y);
            
            return true;
        }
        
        private float GetLabelWidth(Node node)
        {
            SizeF stringSize = painter.MeasureString(node.LabelText);
            var labelWidth = stringSize.Width + painter.NodeHeaderHeight + painter.Margin + painter.TextSideMargin;
            return labelWidth;
        }
    }
}