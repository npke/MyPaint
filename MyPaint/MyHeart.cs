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
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            bool add = false;

            if (lastHeart == null)
            {
                lastHeart = new Heart();
                add = true;
            }

            // Vẽ đối tượng xem trước
            DrawRactangle(lastHeart, shiftKeyPressed);

            // Thêm vào canvas
            if (add)
                collection.Add(lastHeart);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            heart = new Heart();

            // Vẽ đối tượng thực sự
            DrawRactangle(heart, shiftKeyPressed);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastHeart);
            collection.Add(heart);

            DrawedElement = heart;
        }

        private void DrawRactangle(Heart xheart, bool shiftKeyPressed)
        {
            // Xác định tọa độ góc trên bên trái
            var x = Math.Min(StartPoint.X, EndPoint.X);
            var y = Math.Min(StartPoint.Y, EndPoint.Y);

            // Xác định chiều dài, độ rộng của hình
            var w = Math.Max(StartPoint.X, EndPoint.X) - x;
            var h = Math.Max(StartPoint.Y, EndPoint.Y) - y;

            // Kiểm tra người dùng có đang nhấn phím Shift
            if (shiftKeyPressed)
            {
                // Nếu có thực hiện vẽ hình vuông
                xheart.Width = h;
                xheart.Height = h;
            }
            else
            {
                xheart.Width = w;
                xheart.Height = h;
            }

            // Các thuộc tính của đối tượng vẽ
            xheart.Stroke = StrokeBrush;
            xheart.Fill = FillBrush;
            xheart.StrokeThickness = StrokeThickness;

            // Kiểu nét vẽ
            xheart.StrokeDashArray = DashCollection;

            // Định vị trên canvas
            Canvas.SetTop(xheart, StartPoint.Y < EndPoint.Y ? StartPoint.Y : (StartPoint.Y - xheart.Height));
            Canvas.SetLeft(xheart, StartPoint.X < EndPoint.X ? StartPoint.X : (StartPoint.X - xheart.Width));
        }
    }
}
