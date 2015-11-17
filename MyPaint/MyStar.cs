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
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            bool add = false;

            if (lastStar == null)
            {
                lastStar = new Star();
                add = true;
            }

            // Vẽ đối tượng xem trước
            DrawRactangle(lastStar, shiftKeyPressed);

            // Thêm vào canvas
            if (add)
                collection.Add(lastStar);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            star = new Star();

            // Vẽ đối tượng thực sự
            DrawRactangle(star, shiftKeyPressed);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastStar);
            collection.Add(star);

            DrawedElement = star;
        }

        private void DrawRactangle(Star xstar, bool shiftKeyPressed)
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
                xstar.Width = h;
                xstar.Height = h;
            }
            else
            {
                xstar.Width = w;
                xstar.Height = h;
            }

            // Các thuộc tính của đối tượng vẽ
            xstar.Stroke = StrokeBrush;
            xstar.Fill = FillBrush;
            xstar.StrokeThickness = StrokeThickness;

            // Kiểu nét vẽ
            xstar.StrokeDashArray = DashCollection;

            // Định vị trên canvas
            Canvas.SetTop(xstar, StartPoint.Y < EndPoint.Y ? StartPoint.Y : (StartPoint.Y - xstar.Height));
            Canvas.SetLeft(xstar, StartPoint.X < EndPoint.X ? StartPoint.X : (StartPoint.X - xstar.Width));
        }
    }
}
