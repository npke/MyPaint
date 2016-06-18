using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MyPaint
{
    public class CopyCommand : Command
    {
        private CanvasState itemMenu;

        public CopyCommand(CanvasState iMenu)
        {
            itemMenu = iMenu;
        }

        public override void Execute(ref Canvas drawingCanvas,  ref Thumb canvasResizerRightBottom)
        {
            itemMenu.CopyShape(MainWindow.selectedUIElement);
        }
    }
}
