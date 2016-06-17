using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using MyPaintPlugin;

namespace MyPaint
{
    public class MyEllipse : MyShape
    {
        public override void Draw(UIElementCollection collection)
        {
            Ellipse ellipse;
            if (DrawedElement == null)
                ellipse = new Ellipse();
            else ellipse = (Ellipse)DrawedElement;

            UpdateProperties(ellipse);
            LocateShapeOnCanvas(ellipse);

            if (DrawedElement == null)
                collection.Add(ellipse);
            DrawedElement = ellipse;
        }

        public override string GetShapeName()
        {
            return "Ellipse";
        }

        public override MyShape Clone()
        {
            return new MyEllipse();
        }
    } 
}
