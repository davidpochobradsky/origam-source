﻿using System.Drawing;
using System.Windows.Forms;
using Origam.Extensions;
using Origam.Schema.GuiModel;

namespace Origam.Gui.UI
{
    internal static class ToolStripButtonTools
    {
        private const int ImageTextGap = 0;
        public static readonly Size IMAGE_SIZE = new Size(24,24); 
        public static readonly Size BUTTON_SIZE = new Size(24,95); 


        public static void InitBigButton(ToolStripItem actionButton)
        {
            actionButton.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            actionButton.TextAlign = ContentAlignment.BottomCenter;
            actionButton.ImageAlign = ContentAlignment.MiddleCenter;
            actionButton.TextImageRelation = TextImageRelation.ImageAboveText;
            actionButton.AutoSize = true;
            actionButton.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            actionButton.Margin = new Padding(
                left: 5,
                top: 5, 
                right: 5,
                bottom: 20);
            actionButton.Size = BUTTON_SIZE;
            
        }

        public static void InitActionButton(ToolStripItem actionButton
            ,EntityUIAction action)
        {
            actionButton.Text = action.Caption;
            actionButton.Image = action.NodeImage.ToBitmap();
        }

        public static void PaintText(this ToolStripItem actionButton,
            PaintEventArgs e)
        {
            if (actionButton.Owner == null)
                return;
            
            var renderer = actionButton.Owner.Renderer;
            
            bool sholudPaintText = 
                (actionButton.DisplayStyle & ToolStripItemDisplayStyle.Text) ==
                    ToolStripItemDisplayStyle.Text;
            
            if (sholudPaintText){
                renderer.DrawItemText(
                    new ToolStripItemTextRenderEventArgs(
                        e.Graphics,
                        actionButton,
                        actionButton.Text,
                        GetTextRectangle(actionButton),
                        actionButton.ForeColor,
                        actionButton.Font,
                        TextFormatFlags.HorizontalCenter)
                );
            }
        }
        public static void PaintImage(ToolStripItem actionButton,
            PaintEventArgs e, Size imageSize)
        {
            if (actionButton.Owner == null)
                return;
            
            var renderer = actionButton.Owner.Renderer;

            bool shouldPaintImage =
                (actionButton.DisplayStyle & ToolStripItemDisplayStyle.Image) ==
                ToolStripItemDisplayStyle.Image;
            
            if (shouldPaintImage){
                renderer.DrawItemImage(
                    new ToolStripItemImageRenderEventArgs(
                        e.Graphics,
                        actionButton,
                        GetImageRectangle(actionButton,imageSize))
                );
            }
        }

        public static Rectangle GetImageRectangle(ToolStripItem actionButton,
            Size imageSize)
        {
            var xCoord = (actionButton.Width - imageSize.Width) / 2;
            var yCoord = actionButton.Margin.Top; 
            return new Rectangle(
                new Point(xCoord, yCoord),
                imageSize);
        }

        private static Rectangle GetTextRectangle(this ToolStripItem actionButton)
        {
            var textHeight = actionButton.Text.Height(actionButton.Font);
            var textWidth = actionButton.Text.Width(actionButton.Font);
            
            var yCoord = actionButton.Margin.Top  + ImageTextGap 
                         + textHeight;
            var xCoord = (actionButton.Width - textWidth) / 2;

            return new Rectangle(xCoord, yCoord, textWidth, textHeight);
        }
    }
}