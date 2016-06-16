using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyPaint
{
    class MyHeart : MyShape
    {
        private Heart lastHeart, heart;

        // Vẽ khi chuột nhấn giữ chuột và di chuyển (vẽ xem trước)
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            bool add = false;

            if (lastHeart == null)
            {
                lastHeart = new Heart();
                add = true;
            }

            // Vẽ đối tượng xem trước
            Draw(lastHeart, shiftKey);

            // Thêm vào canvas
            if (add)
                collection.Add(lastHeart);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            heart = new Heart();

            // Vẽ đối tượng thực sự
            Draw(heart, shiftKey);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastHeart);
            collection.Add(heart);

            DrawedElement = heart;
        }
    }
}
