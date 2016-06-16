using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyPaint
{
    // Lớp MyShape là lớp trừu tượng dùng để quản lý các đối tượng đồ họa như Line, Rectangle, Ellipse
    public abstract class MyShape
    {
        // Handle để quản lý đối tượng trong ShapeManager
        public int Handle { get; private set; }

        // Màu đường viền nét vẽ
        public Brush StrokeBrush { get; set; }

        // Độ dày nét vẽ
        public double StrokeThickness { get; set; }

        // Màu tô bên trong
        public Brush FillBrush { get; set; }

        // Đường nét đứt
        public DoubleCollection DashCollection { get; set; }

        // Điểm bắt đầu vẽ
        public Point StartPoint { get; set; }

        // Điểm kết thúc vẽ
        public Point EndPoint { get; set; }

        // Đối tượng đã vẽ
        public UIElement DrawedElement { get; set; }

        // Phương thức vẽ khi di chuyển chuột
        public abstract void DrawOnMouseMove(UIElementCollection collection, bool shiftKey);

        // Phương thức vẽ khi nhả chuột
        public abstract void DrawOnMouseUp(UIElementCollection collection, bool shiftKey);

        // Phương thức vẽ đối tượng
        public virtual void Draw(Shape shape, bool shiftKey)
        {
            // Xác định tọa độ góc trên bên trái
            var x = Math.Min(StartPoint.X, EndPoint.X);
            var y = Math.Min(StartPoint.Y, EndPoint.Y);

            // Xác định chiều dài, độ rộng của hình
            var w = Math.Max(StartPoint.X, EndPoint.X) - x;
            var h = Math.Max(StartPoint.Y, EndPoint.Y) - y;

            // Kiểm tra người dùng có đang nhấn phím Shift
            if (shiftKey)
            {
                // Nếu có thực hiện vẽ hình vuông
                shape.Width = h;
                shape.Height = h;
            }
            else
            {
                shape.Width = w;
                shape.Height = h;
            }

            // Các thuộc tính của đối tượng vẽ
            shape.Stroke = StrokeBrush;
            shape.Fill = FillBrush;
            shape.StrokeThickness = StrokeThickness;

            // Kiểu nét vẽ
            shape.StrokeDashArray = DashCollection;

            // Định vị trên canvas
            Canvas.SetTop(shape, StartPoint.Y < EndPoint.Y ? StartPoint.Y : (StartPoint.Y - shape.Height));
            Canvas.SetLeft(shape, StartPoint.X < EndPoint.X ? StartPoint.X : (StartPoint.X - shape.Width));
        }

        // Constructor
        public MyShape()
        {
            this.Handle = ShapeManager.GetNextHandle();
            ShapeManager.AddShape(this);
        }

        // Phương thức xóa bỏ đối tượng đã vẽ
        public void Remove(System.Windows.Controls.UIElementCollection collection)
        {
            if (DrawedElement != null)
                collection.Remove(DrawedElement);
        }
    }
}
