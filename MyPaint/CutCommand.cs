using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace MyPaint
{
    public class CutCommand : Command
    {
        private CanvasState itemMenu;

        public CutCommand(CanvasState iMenu)
        {
            itemMenu = iMenu;
        }

        public override void Execute(ref Canvas drawingCanvas, ref Thumb canvasResizerRightBottom, string fileName, RenderTargetBitmap renderBitmap)
        {
            itemMenu.CutShape(MainWindow.selectedUIElement,ref drawingCanvas);
        }
    }
}
