using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace MyPaint
{
    public class MyEllipse : MyShape
    {
        Ellipse lastEllipse, ellipse;

        // Vẽ hình ellipse xem trước khi di chuyển chuột 
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            bool add = false;

            if (lastEllipse == null)
            {
                lastEllipse = new Ellipse();
                add = true;
            }

            // Vẽ đối tượng xem trước
            DrawEllipse(lastEllipse, shiftKeyPressed);
           	
           	// Thêm vào canvas
            if (add)
                collection.Add(lastEllipse);
        }

        // Phương thức vẽ đối tượng cuối cùng thực sự khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            ellipse = new Ellipse();

            // Vẽ đối tượng thực sự
            DrawEllipse(ellipse, shiftKeyPressed);

            // Xóa đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastEllipse);
            collection.Add(ellipse);

            DrawedElement = ellipse;
        }

        private void DrawEllipse(Ellipse xEllipse, bool shiftKeyPressed)
        {
            // Xác định tọa độ góc trên bên trái của hình chữ nhật chứa hình ellipse
            var x = Math.Min(StartPoint.X, EndPoint.X);
            var y = Math.Min(StartPoint.Y, EndPoint.Y);

            // Xác định chiều dài, độ rộng của hình
            var w = Math.Max(StartPoint.X, EndPoint.X) - x;
            var h = Math.Max(StartPoint.Y, EndPoint.Y) - y;

            // Kiểm tra người dùng có đang nhấn phím Shift
            if (shiftKeyPressed)
            {
                // Nếu có thực hiện vẽ hình tròn
                xEllipse.Width = h;
                xEllipse.Height = h;
            }
            else
            {
                xEllipse.Width = w;
                xEllipse.Height = h;
            }

            // Các thuộc tính của đối tượng vẽ
            xEllipse.Stroke = StrokeBrush;
            xEllipse.Fill = FillBrush;
            xEllipse.StrokeThickness = StrokeThickness;

            // Kiể nét vẽ
            xEllipse.StrokeDashArray = DashCollection;

            // Định vị trên canvas
            Canvas.SetTop(xEllipse, StartPoint.Y < EndPoint.Y ? StartPoint.Y : (StartPoint.Y - xEllipse.Height));
            Canvas.SetLeft(xEllipse, StartPoint.X < EndPoint.X ? StartPoint.X : (StartPoint.X - xEllipse.Width));
        }
    } 
}
