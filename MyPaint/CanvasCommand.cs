using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MyPaint
{
    public class CanvasCommand : Command
    {
        CanvasState canvasState = null;

        public CanvasCommand(CanvasState canvas)
        {
            canvasState = canvas;
        }

        public override void Undo(ref Canvas DrawingCanvas)
        {
            canvasState.RestoreCanvas(ref  DrawingCanvas);
        }

        public override void Redo(ref Canvas drawingCanvas)
        {
            canvasState.RestoreCanvas(ref drawingCanvas);
        }
    }
}
