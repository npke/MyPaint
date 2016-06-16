using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace MyPaint
{
    public class MyRectangle : MyShape
    {
        private Rectangle lastRectangle, rectangle;

        // Vẽ khi chuột nhấn giữ chuột và di chuyển (vẽ xem trước)
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            bool add = false;

            if (lastRectangle == null)
            {
                lastRectangle = new Rectangle();
                add = true;
            }

            // Vẽ đối tượng xem trước
            Draw(lastRectangle, shiftKey);

            // Thêm vào canvas
            if (add)
                collection.Add(lastRectangle);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            rectangle = new Rectangle();

            // Vẽ đối tượng thực sự
            Draw(rectangle, shiftKey);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastRectangle);
            collection.Add(rectangle);

            DrawedElement = rectangle;
        }
    }
}
