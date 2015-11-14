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
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            bool add = false;

            if (lastRectangle == null)
            {
                lastRectangle = new Rectangle();
                add = true;
            }

            // Vẽ đối tượng xem trước
            DrawRactangle(lastRectangle, shiftKeyPressed);

            // Thêm vào canvas
            if (add)
                collection.Add(lastRectangle);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            rectangle = new Rectangle();

            // Vẽ đối tượng thực sự
            DrawRactangle(rectangle, shiftKeyPressed);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastRectangle);
            collection.Add(rectangle);

            DrawedElement = rectangle;
        }

        private void DrawRactangle(Rectangle xRectangle, bool shiftKeyPressed)
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
                xRectangle.Width = h;
                xRectangle.Height = h;
            }
            else
            {
                xRectangle.Width = w;
                xRectangle.Height = h;
            }

            // Các thuộc tính của đối tượng vẽ
            xRectangle.Stroke = StrokeBrush;
            xRectangle.Fill = FillBrush;
            xRectangle.StrokeThickness = StrokeThickness;

            // Kiểu nét vẽ
            xRectangle.StrokeDashArray = DashCollection;

            // Định vị trên canvas
            Canvas.SetTop(xRectangle, StartPoint.Y < EndPoint.Y ? StartPoint.Y : (StartPoint.Y - xRectangle.Height));
            Canvas.SetLeft(xRectangle, StartPoint.X < EndPoint.X ? StartPoint.X : (StartPoint.X - xRectangle.Width));
        }
    }
}
