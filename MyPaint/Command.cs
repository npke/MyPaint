using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MyPaint
{
    public abstract class Command
    {
        public virtual void Undo(ref Canvas DrawingCanvas)
        { }

        public virtual void Redo(ref Canvas DrawingCanvas)
        { }

        public virtual void Execute(ref Canvas drawingCanvas, ref Thumb canvasResizerRightBottom)
        { }
    }
}
