using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyPaint
{
    class MyArrow : MyShape
    {
        public override void Draw(UIElementCollection collection)
        {
            Arrow arrow;
            if (DrawedElement == null)
                arrow = new Arrow();
            else arrow = (Arrow)DrawedElement;

            UpdateProperties(arrow);
            LocateShapeOnCanvas(arrow);

            if (DrawedElement == null)
                collection.Add(arrow);
            DrawedElement = arrow;
        }
    }
}
