using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyPaint
{
    public class CanvasState
    {
        private List<UIElement> _ListUIE = new List<UIElement>();

        public CanvasState(List<UIElement> ListUIE)
        {
            _ListUIE = ListUIE;
        }

        // Thực hiện khôi phục lại trạng thái đó
        public void RestoreCanvas(ref Canvas drawingCanvas )
        {
            drawingCanvas.Children.Clear();
            foreach (UIElement UIE in _ListUIE)
            {
                drawingCanvas.Children.Add(UIE);
            }
        }
    }
}
