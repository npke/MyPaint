using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyPaint
{
    // Lớp biểu diễn đối tượng hình mũi tên
    public class Arrow : Shape
    {
        public Arrow()
        {            
        }

        // Cài đặt các đường Path nối nhau để tạo nên hình mũi tên
        protected override Geometry DefiningGeometry
        {
            get 
            {
                Point A = new Point(0 + 1, this.Height / 4 - 1);
                Point B = new Point(0 + 1, 3 * this.Height / 4 - 1);
                Point C = new Point(2 * this.Width / 3 - 1, 0 + 1);
                Point D = new Point(2 * this.Width / 3 - 1, this.Height / 4 - 1);
                Point E = new Point(2 * this.Width / 3 - 1, 3 * this.Height / 4 - 1);
                Point F = new Point(2 * this.Width / 3 - 1, this.Height - 1);
                Point G = new Point(this.Width - 1, this.Height / 2);

                List<PathSegment> segments = new List<PathSegment>(7);
                segments.Add(new LineSegment(A, true));
                segments.Add(new LineSegment(B, true));
                segments.Add(new LineSegment(E, true));
                segments.Add(new LineSegment(F, true));
                segments.Add(new LineSegment(G, true));
                segments.Add(new LineSegment(C, true));
                segments.Add(new LineSegment(D, true));
                
                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(A, segments, true);
                figures.Add(pf);

                Geometry g = new PathGeometry(figures);
                return g;
            }
        }
    }
}
