using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyPaint
{
    class MyStar : MyShape
    {
        public override void Draw(UIElementCollection collection)
        {
            Star star;
            if (DrawedElement == null)
                star = new Star();
            else star = (Star)DrawedElement;

            UpdateProperties(star);
            LocateShapeOnCanvas(star);

            if (DrawedElement == null)
                collection.Add(star);
            DrawedElement = star;
        }
    }
}
