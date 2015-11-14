using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace MyPaint
{
    // Lớp MyShape là lớp trừu tượng dùng để quản lý các đối tượng đồ họa như Line, Rectangle, Ellipse
    public abstract class MyShape
    {
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
        public abstract void DrawOnMouseMove(UIElementCollection collection, bool shiftKeyPressed);

        // Phương thức vẽ khi nhả chuột
        public abstract void DrawOnMouseUp(UIElementCollection collection, bool shiftKeyPressed);

        // Phương thức xóa bỏ đối tượng đã vẽ
        public void Remove(System.Windows.Controls.UIElementCollection collection)
        {
            if (DrawedElement != null)
                collection.Remove(DrawedElement);
        }
    }
}
