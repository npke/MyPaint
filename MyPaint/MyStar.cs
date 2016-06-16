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
        private Star lastStar, star;

        // Vẽ khi chuột nhấn giữ chuột và di chuyển (vẽ xem trước)
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            bool add = false;

            if (lastStar == null)
            {
                lastStar = new Star();
                add = true;
            }

            // Vẽ đối tượng xem trước
            Draw(lastStar, shiftKey);

            // Thêm vào canvas
            if (add)
                collection.Add(lastStar);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKey)
        {
            star = new Star();

            // Vẽ đối tượng thực sự
            Draw(star, shiftKey);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastStar);
            collection.Add(star);

            DrawedElement = star;
        }
    }
}
