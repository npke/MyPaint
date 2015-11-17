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
    public class PTDuongThang
    {
       public double a {get; set;}
       public double b {get; set;}
       public double c {get; set;}


        public PTDuongThang()
        {
            a = b = c = 0;
        }

        public PTDuongThang(Point M, Point N)
        {
            Vector VTPT = new Vector(-(N.Y - M.Y), (N.X - M.X));
            a = VTPT.X;
            b = VTPT.Y;
            c = M.Y * VTPT.Y + M.X * VTPT.X;
        }
    }

    public class Star : Shape
    {
        public Star()
        {            
        }
        private Point getIntersectionPoint(PTDuongThang A, PTDuongThang B)
        {
            double delta = A.a * B.b - B.a * A.b;
            if (delta == 0)
                return new Point(0,0);

            double x = (B.b * A.c - A.b * B.c) / delta;
            double y = (A.a * B.c - B.a * A.c) / delta;

            return new Point(x, y);
        }
        protected override Geometry DefiningGeometry
        {
            get 
            {
                Point A = new Point(this.Width / 2, 0);
                Point C = new Point(0, this.Height / 2.618);
                Point E = new Point(0 + this.Width / 6, this.Height);
                Point G = new Point(this.Width - this.Width / 6, this.Height);
                Point H = new Point(this.Width, this.Height / 2.618);

                PTDuongThang PTAE = new PTDuongThang(A, E);
                PTDuongThang PTCH = new PTDuongThang(C, H);

                PTDuongThang PTCG = new PTDuongThang(C, G);

                PTDuongThang PTEH = new PTDuongThang(E, H);

                PTDuongThang PTAG = new PTDuongThang(A, G);

                Point B = getIntersectionPoint(PTAE, PTCH);
                Point D = getIntersectionPoint(PTAE, PTCG);
                Point F = getIntersectionPoint(PTCG, PTEH);
                Point J = getIntersectionPoint(PTEH, PTAG);
                Point I = getIntersectionPoint(PTAG, PTCH);


                List<PathSegment> segments = new List<PathSegment>(3);
                segments.Add(new LineSegment(A, true));
                segments.Add(new LineSegment(B, true));
                segments.Add(new LineSegment(C, true));
                segments.Add(new LineSegment(D, true));
                segments.Add(new LineSegment(E, true));
                segments.Add(new LineSegment(F, true));
                segments.Add(new LineSegment(G, true));
                segments.Add(new LineSegment(J, true));
                segments.Add(new LineSegment(H, true));
                segments.Add(new LineSegment(I, true));

                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(A, segments, true);
                figures.Add(pf);

                Geometry g = new PathGeometry(figures);
                return g;
            }
        }
    }
}
