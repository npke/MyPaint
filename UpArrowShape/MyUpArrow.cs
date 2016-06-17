using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPaintPlugin;

namespace UpArrowShape
{
    public class MyUpArrow : MyShape
    {
        public override void Draw(System.Windows.Controls.UIElementCollection collection)
        {
            UpArrow upArrow;
            if (DrawedElement == null)
                upArrow = new UpArrow();
            else upArrow = (UpArrow)DrawedElement;

            UpdateProperties(upArrow);
            LocateShapeOnCanvas(upArrow);

            if (DrawedElement == null)
                collection.Add(upArrow);
            DrawedElement = upArrow;
        }

        public override string GetShapeName()
        {
            return "Up Arrow";
        }

        public override MyShape Clone()
        {
            return new MyUpArrow();
        }
    }
}
