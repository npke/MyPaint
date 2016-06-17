using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MyPaintPlugin;

namespace MyPaint
{
    class MyTriangle : MyShape
    {
        public override void Draw(UIElementCollection collection)
        {
            Triangle triangle;
            if (DrawedElement == null)
                triangle = new Triangle();
            else triangle = (Triangle)DrawedElement;

            UpdateProperties(triangle);
            LocateShapeOnCanvas(triangle);

            if (DrawedElement == null)
                collection.Add(triangle);
            DrawedElement = triangle;
        }
    }
}
