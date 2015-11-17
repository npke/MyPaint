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
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            bool add = false;

            if (lastArrow == null)
            {
                lastArrow = new Arrow();
                add = true;
            }

            // Vẽ đối tượng xem trước
            DrawRactangle(lastArrow, shiftKeyPressed);

            // Thêm vào canvas
            if (add)
                collection.Add(lastArrow);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            arrow = new Arrow();

            // Vẽ đối tượng thực sự
            DrawRactangle(arrow, shiftKeyPressed);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastArrow);
            collection.Add(arrow);

            DrawedElement = arrow;
        }

        private void DrawRactangle(Arrow xArrow, bool shiftKeyPressed)
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
                xArrow.Width = h;
                xArrow.Height = h;
            }
            else
            {
                xArrow.Width = w;
                xArrow.Height = h;
            }

            // Các thuộc tính của đối tượng vẽ
            xArrow.Stroke = StrokeBrush;
            xArrow.Fill = FillBrush;
            xArrow.StrokeThickness = StrokeThickness;

            // Kiểu nét vẽ
            xArrow.StrokeDashArray = DashCollection;

            // Định vị trên canvas
            Canvas.SetTop(xArrow, StartPoint.Y < EndPoint.Y ? StartPoint.Y : (StartPoint.Y - xArrow.Height));
            Canvas.SetLeft(xArrow, StartPoint.X < EndPoint.X ? StartPoint.X : (StartPoint.X - xArrow.Width));
        }
    }
}
