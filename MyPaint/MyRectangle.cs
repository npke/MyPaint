using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;

// 01676489651

namespace MyPaint
{
    public class MyRectangle : MyShape
    {
        public override void Draw(UIElementCollection collection)
        {
            Rectangle rectangle;
            if (DrawedElement == null)
                rectangle = new Rectangle();
            else rectangle = (Rectangle)DrawedElement;

            UpdateProperties(rectangle);
            LocateShapeOnCanvas(rectangle);

            if (DrawedElement == null)
                collection.Add(rectangle);
            DrawedElement = rectangle;
        }
    }
}
