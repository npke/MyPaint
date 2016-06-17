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
    class MyLine : MyShape
    {
        public override void Draw(UIElementCollection collection)
        {
            Line line;
            if (DrawedElement == null)
                line = new Line();
            else line = (Line)DrawedElement;

            UpdateProperties(line);

            if (DrawedElement == null)
                collection.Add(line);

            DrawedElement = line;
        }

        public override void UpdateProperties(Shape shape)
        {
            Line line = (Line)shape;
            // Điểm đầu
            line.X1 = StartPoint.X;
            line.Y1 = StartPoint.Y;

            // Điểm cuối
            line.X2 = EndPoint.X;
            line.Y2 = EndPoint.Y;

            line.Stroke = StrokeBrush;
            line.StrokeThickness = StrokeThickness;

            // Kiểu nét vẽ
            line.StrokeDashArray = DashCollection;
        }

        public override string GetShapeName()
        {
            return "Line";
        }


        public override MyShape Clone()
        {
            return new MyLine();
        }
    }
}
