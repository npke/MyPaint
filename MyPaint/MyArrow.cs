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
        private Arrow lastArrow, arrow;

        // Vẽ khi chuột nhấn giữ chuột và di chuyển (vẽ xem trước)
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            bool add = false;

            if (lastArrow == null)
            {
                lastArrow = new Arrow();
                add = true;
            }

            // Vẽ đối tượng xem trước
            Draw(lastArrow, shiftKey);

            // Thêm vào canvas
            if (add)
                collection.Add(lastArrow);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            arrow = new Arrow();

            // Vẽ đối tượng thực sự
            Draw(arrow, shiftKey);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastArrow);
            collection.Add(arrow);

            DrawedElement = arrow;
        }
    }
}
