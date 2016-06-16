using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace MyPaint
{
    public class MyEllipse : MyShape
    {
        Ellipse lastEllipse, ellipse;

        // Vẽ hình ellipse xem trước khi di chuyển chuột 
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            bool add = false;

            if (lastEllipse == null)
            {
                lastEllipse = new Ellipse();
                add = true;
            }

            // Vẽ đối tượng xem trước
            Draw(lastEllipse, shiftKey);
           	
           	// Thêm vào canvas
            if (add)
                collection.Add(lastEllipse);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            ellipse = new Ellipse();

            // Vẽ đối tượng thực sự
            Draw(ellipse, shiftKey);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastEllipse);
            collection.Add(ellipse);

            DrawedElement = ellipse;
        }
    } 
}
