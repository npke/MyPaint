using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyPaint
{
    // Lớp biểu diễn đối tượng hình tam giác
    public class Triangle : Shape
    {
        // Phương thức khởi tạo
        public Triangle()
        {            
        }

        // Cài đặt xử lý 
        protected override Geometry DefiningGeometry
        {
            get 
            {
                Point A = new Point(this.Width / 2 - 1, 1);
                Point B = new Point(1, this.Height - 1);
                Point C = new Point(this.Width - 1, this.Height - 1);

                List<PathSegment> segments = new List<PathSegment>(3);
                segments.Add(new LineSegment(C, true));
                segments.Add(new LineSegment(B, true));
                segments.Add(new LineSegment(A, true));

                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(A, segments, true);
                figures.Add(pf);

                Geometry g = new PathGeometry(figures);
                return g;
            }
        }
    }
}
