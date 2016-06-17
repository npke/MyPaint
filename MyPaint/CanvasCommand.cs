using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MyPaint
{
    public class CanvasCommand : Command
    {
        CanvasState cvs = null;

        public CanvasCommand(CanvasState canvas)
        {
            cvs = canvas;
        }



        public override void Undo(ref Canvas DrawingCanvas)
        {
            cvs.RestoreCanvas(ref  DrawingCanvas);

        }

        public override void Redo(ref Canvas drawingCanvas)
        {
            cvs.RestoreCanvas(ref drawingCanvas);
        }
    }
}
