using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyPaint
{
    class MyTriangle : MyShape
    {
        private Triangle lastTriangle, triangle;

        // Vẽ khi chuột nhấn giữ chuột và di chuyển (vẽ xem trước)
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            bool add = false;

            if (lastTriangle == null)
            {
                lastTriangle = new Triangle();
                add = true;
            }

            // Vẽ đối tượng xem trước
            Draw(lastTriangle, shiftKey);

            // Thêm vào canvas
            if (add)
                collection.Add(lastTriangle);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            triangle = new Triangle();

            // Vẽ đối tượng thực sự
            Draw(triangle, shiftKey);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastTriangle);
            collection.Add(triangle);

            DrawedElement = triangle;
        }
    }
}
