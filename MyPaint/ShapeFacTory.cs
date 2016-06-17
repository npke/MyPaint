using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPaintPlugin;
using System.Windows;
using System.Windows.Media;

namespace MyPaint
{
    public class ShapeFactory
    {

        // Shape prototypes
        private static Dictionary<string, MyShape> prototypes = new Dictionary<string, MyShape>();

        // Add new prototype
        public static bool AddPrototype(string shapeName, MyShape shape)
        {
            if (!prototypes.Keys.Contains(shapeName))
            {
                prototypes.Add(shapeName, shape);
                return true;
            }

            return false;
        }

        // Get shape from prototype list
        public static MyShape GetShape(string shapeName)
        {
            if (prototypes.ContainsKey(shapeName))
                return prototypes[shapeName].Clone();
            return null;
        }

        // Populate built-in shape
        public static void PopulateBuiltInShape()
        {
            prototypes.Add("Line", new MyLine());
            prototypes.Add("Arrow", new MyArrow());
            prototypes.Add("Ellipse", new MyEllipse());
            prototypes.Add("Rectangle", new MyRectangle());
            prototypes.Add("Star", new MyStar());
            prototypes.Add("Triangle", new MyTriangle());
            prototypes.Add("Heart", new MyHeart());
        }
    }
}
