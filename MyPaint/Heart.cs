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
    public class Heart : Shape
    {
        public Heart()
        {
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                Point A = new Point(this.Width / 2, this.Height / 4);
                Point B = new Point(0, this.Height / 3);
                Point C = new Point(this.Width / 2, this.Height);
                Point B1 = new Point((B.X + C.X) / 6, (B.Y + C.Y) / 2);
                Point D = new Point(this.Width, this.Height / 3);
                Point D1 = new Point( D.X - B1.X, B1.Y);

                List<PathSegment> segments = new List<PathSegment>(4);
                
                segments.Add(new ArcSegment(B, new Size(1, 1), 0, true, SweepDirection.Counterclockwise, true));
                //segments.Add(new QuadraticBezierSegment(B, C, true));
                segments.Add(new BezierSegment(B, B1, C, true));
                segments.Add(new BezierSegment(C, D1, D, true));
                //segments.Add(new QuadraticBezierSegment(C, D, true));
                segments.Add(new ArcSegment(A, new Size(1, 1), 0, true, SweepDirection.Counterclockwise, true));
                

                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(A, segments, true);
                figures.Add(pf);

                Geometry g = new PathGeometry(figures);
                return g;
            }
        }
    }
}
