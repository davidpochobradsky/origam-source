using System.Drawing;
using Microsoft.Msagl.Drawing;
using Origam.Workbench.Diagram.Extensions;

namespace Origam.Workbench.Diagram.NodeDrawing
{
    class NodeHeaderPainter
    {
        private readonly InternalPainter painter;
        private readonly bool isFromActivePackage;

        public NodeHeaderPainter(InternalPainter painter,
            bool isFromActivePackage)
        {
            this.painter = painter;
            this.isFromActivePackage = isFromActivePackage;
        }

        public void Draw(Node node, Graphics editorGraphics, Rectangle border)
        {
            NodeImages images = painter.GetImages(node);

            SizeF stringSize =
                editorGraphics.MeasureString(node.LabelText, painter.Font);
            
            Rectangle imageBackground = new Rectangle(border.Location,
                new Size(painter.NodeHeaderHeight, painter.NodeHeaderHeight));

            Point headerCenter = border.GetCenter();
            var labelPoint = new PointF(
                headerCenter.X - (float) border.Width / 2 +
                painter.NodeHeaderHeight + painter.TextSideMargin + (images.Secondary == null ? 0 : imageBackground.Width - 5),
                (float) headerCenter.Y -
                (int) stringSize.Height / 2);

            var imageBorder = new Size(
                (imageBackground.Width - images.Primary.Width) / 2,
                (imageBackground.Height - images.Primary.Height) / 2);
            var primaryImagePoint = new PointF(
                headerCenter.X - (float) border.Width / 2 +
                imageBorder.Width,
                headerCenter.Y -
                (float) border.Height / 2 + imageBorder.Height);
            
            var secondaryImagePoint = new PointF(
                headerCenter.X - (float) border.Width / 2 +
                imageBorder.Width  + imageBackground.Width,
                headerCenter.Y -
                (float) border.Height / 2 + imageBorder.Height);
            
            
            
            editorGraphics.DrawUpSideDown(drawAction: graphics =>
                {
                    graphics.DrawString(node.LabelText, painter.Font, painter.GetTextBrush(isFromActivePackage),
                        labelPoint, painter.DrawFormat);
                    graphics.FillRectangle(painter.LightGreyBrush, imageBackground);
                    graphics.DrawImage(images.Primary, primaryImagePoint);
                    if (images.Secondary != null)
                    {
                        graphics.DrawImage(images.Secondary, secondaryImagePoint);
                    }
                },
                yAxisCoordinate: headerCenter.Y);
        }
    }
}