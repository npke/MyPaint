using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HinhMessage
{
    public class RectangularCallout : Shape
    {
        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                Point A = new Point(1, 1);
                Point B = new Point(this.Width - 1, 1);
                Point C = new Point(this.Width - 1, 4*(this.Height/5) - 1);
                Point D = new Point((this.Width/5) * 2 - 1, 4*(this.Height/5) - 1);
                Point E = new Point((this.Width / 5) * 1.5 - 1, this.Height - 1);
                Point F = new Point(this.Width / 5 - 1, 4 * (this.Height / 5) - 1);
                Point G = new Point(1, 4 * (this.Height / 5) - 1);

                List<PathSegment> segments = new List<PathSegment>(7);
                segments.Add(new LineSegment(A, true));
                segments.Add(new LineSegment(B, true));
                segments.Add(new LineSegment(C, true));
                segments.Add(new LineSegment(D, true));
                segments.Add(new LineSegment(E, true));
                segments.Add(new LineSegment(F, true));
                segments.Add(new LineSegment(G, true));

                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pathfigure = new PathFigure(A, segments, true);
                figures.Add(pathfigure);

                Geometry g = new PathGeometry(figures);
                return g; 
            }
        }
    }
}
