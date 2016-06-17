using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MyPaintPlugin;

namespace MyPaint
{
    class MyHeart : MyShape
    {
        public override void Draw(UIElementCollection collection)
        {
            Heart heart;
            if (DrawedElement == null)
                heart = new Heart();
            else heart = (Heart)DrawedElement;

            UpdateProperties(heart);
            LocateShapeOnCanvas(heart);

            if (DrawedElement == null)
                collection.Add(heart);
            DrawedElement = heart;
        }
    }
}
