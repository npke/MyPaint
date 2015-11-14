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
    class MyLine : MyShape
    {
        private Line lastLine, line;

        // Vẽ khi chuột nhấn giữ chuột và di chuyển (vẽ xem trước)
        public override void DrawOnMouseMove(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            bool add = false;

            if (lastLine == null)
            {
                lastLine = new Line();
                add = true;
            }

            // Gán các thuộc tính
            SetProperties(lastLine);

            // Thêm vào canvas
            if (add && !StartPoint.Equals(EndPoint))
                collection.Add(lastLine);
        }


        // Vẽ đối tượng thật sự cuối cùng khi nhả chuột
        public override void DrawOnMouseUp(System.Windows.Controls.UIElementCollection collection, bool shiftKeyPressed)
        {
            // Khởi tạo một line mới
            line = new Line();

            // Thiết lập các thuộc tính
            SetProperties(line);

            // Xóa bỏ đối tượng vẽ xem trước và thêm đối tượng thật sự vào canvas
            collection.Remove(lastLine);
            if (!StartPoint.Equals(EndPoint))
            {
                collection.Add(line);
                DrawedElement = line;
            }
        }

        private void SetProperties(Line xLine)
        {
            // Điểm đầu
            xLine.X1 = StartPoint.X;
            xLine.Y1 = StartPoint.Y;

            // Điểm cuối
            xLine.X2 = EndPoint.X;
            xLine.Y2 = EndPoint.Y;

            xLine.Stroke = StrokeBrush;
            xLine.StrokeThickness = StrokeThickness;

            // Kiểu nét vẽ
            xLine.StrokeDashArray = DashCollection;
        }
    }
}
