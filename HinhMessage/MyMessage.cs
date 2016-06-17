using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using MyPaintPlugin;

namespace HinhMessage
{
    public class MyMessage : MyShape
    {
        public override void Draw(System.Windows.Controls.UIElementCollection collection)
        {
            RectangularCallout callout;
            if (DrawedElement == null)
                callout = new RectangularCallout();
            else callout = (RectangularCallout)DrawedElement;

            UpdateProperties(callout);
            LocateShapeOnCanvas(callout);

            if (DrawedElement == null)
                collection.Add(callout);
            DrawedElement = callout;
        }

        public override string GetShapeName()
        {
            return "Rectanglar Callout";
        }

        public override MyShape Clone()
        {
            return new MyMessage();
        }
    }
}
