using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MyPaint
{
    public abstract class Command
    {
        public abstract void Undo(ref Canvas DrawingCanvas);

        public abstract void Redo(ref Canvas DrawingCanvas);
    }
}
