using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Origam.Schema;
using Origam.Workbench.Diagram.Extensions;
using Point = Microsoft.Msagl.Core.Geometry.Point;

namespace Origam.Workbench.Diagram.DiagramFactory
{
    class NodeFactory: IDisposable
    {
        private readonly int margin = 3;
        private readonly int marginLeft = 5;
        private readonly Font font = new Font("Arial", 12);
        private readonly SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);
        private readonly StringFormat drawFormat = new StringFormat();
        private readonly Graphics measurementGraphics = new Control().CreateGraphics();
        private readonly Pen boldBlackPen = new Pen(System.Drawing.Color.Black, 2);
        private readonly Pen blackPen =new Pen(System.Drawing.Color.Black, 1);
        private readonly GViewer viewer;

        public NodeFactory(GViewer viewer)
        {
            this.viewer = viewer;
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
 
        private ICurve GetNodeBoundary(Node node) {
            var image = GetImage(node);
            var borderSize = CalculateBorderSize(node, image);
            return CurveFactory.CreateRectangle(borderSize.Width, borderSize.Height, new Point());
        }

        private bool DrawNode(Node node, object graphicsObj) {
            Graphics editorGraphics = (Graphics)graphicsObj;
            var image = GetImage(node);

            Pen pen = viewer.SelectedObject == node
                ? boldBlackPen 
                : blackPen;
            
            SizeF stringSize = editorGraphics.MeasureString(node.LabelText, font);

            var borderSize = CalculateBorderSize(node, image);
            var borderCorner = new System.Drawing.Point(
                (int)node.GeometryNode.Center.X - borderSize.Width / 2,
                (int)node.GeometryNode.Center.Y - borderSize.Height / 2);
            Rectangle border = new Rectangle(borderCorner, borderSize);

            var labelPoint = new PointF(
                (float)node.GeometryNode.Center.X - (float)border.Width / 2 + image.Width + margin * 2,
                (float)node.GeometryNode.Center.Y - stringSize.Height / 2);

            var imagePoint = new PointF(
                (float)(node.GeometryNode.Center.X - (float)border.Width / 2 + marginLeft),
                (float)(node.GeometryNode.Center.Y - (float)image.Height / 2));

            editorGraphics.DrawUpSideDown(drawAction: graphics =>
                {
                    graphics.DrawString(node.LabelText, font, drawBrush,
                        labelPoint, drawFormat);
                    graphics.DrawRectangle(pen, border);
                    graphics.DrawImage(image, imagePoint);
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

        private Size CalculateBorderSize(Node node, Image image)
        {
            SizeF stringSize = measurementGraphics.MeasureString(node.LabelText, font);

            int totalWidth = (int) (margin + stringSize.Width + margin + image.Width + margin);
            int totalHeight = stringSize.Height > image.Height
                ? (int) stringSize.Height + margin * 2
                : image.Height + margin * 2;
            return  new Size(totalWidth, totalHeight);
        }

        public void Dispose()
        {
            font?.Dispose();
            blackPen?.Dispose();
            drawBrush?.Dispose();
            drawFormat?.Dispose();
            boldBlackPen?.Dispose();
            measurementGraphics?.Dispose();
        }
    }
}