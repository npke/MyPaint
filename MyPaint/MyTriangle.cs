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
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            bool add = false;

            if (lastTriangle == null)
            {
                lastTriangle = new Triangle();
                add = true;
            }

            // Vẽ đối tượng xem trước
            DrawRactangle(lastTriangle, shiftKeyPressed);

            // Thêm vào canvas
            if (add)
                collection.Add(lastTriangle);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            triangle = new Triangle();

            // Vẽ đối tượng thực sự
            DrawRactangle(triangle, shiftKeyPressed);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastTriangle);
            collection.Add(triangle);

            DrawedElement = triangle;
        }

        private void DrawRactangle(Triangle xtriangle, bool shiftKeyPressed)
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
                xtriangle.Width = h;
                xtriangle.Height = h;
            }
            else
            {
                xtriangle.Width = w;
                xtriangle.Height = h;
            }

            // Các thuộc tính của đối tượng vẽ
            xtriangle.Stroke = StrokeBrush;
            xtriangle.Fill = FillBrush;
            xtriangle.StrokeThickness = StrokeThickness;

            // Kiểu nét vẽ
            xtriangle.StrokeDashArray = DashCollection;

            // Định vị trên canvas
            Canvas.SetTop(xtriangle, StartPoint.Y < EndPoint.Y ? StartPoint.Y : (StartPoint.Y - xtriangle.Height));
            Canvas.SetLeft(xtriangle, StartPoint.X < EndPoint.X ? StartPoint.X : (StartPoint.X - xtriangle.Width));
        }
    }
}
