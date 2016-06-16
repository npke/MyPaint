using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MyPaint
{
    

    public class ShapeFactory
    {

        // Protoypes shape
        private static Dictionary<ShapeType.SHAPE, MyShape> Prototypes = new Dictionary<ShapeType.SHAPE, MyShape>();

        public static void PopulateAllShapeInstances()
        {
            var ListType = Enum.GetValues(typeof(ShapeType.SHAPE));
            foreach (ShapeType.SHAPE item in ListType)
                CreateInstanceOf(item);

        }



        // Hàm tạo mới một shape và add vào danh sách prototype 
        private static void CreateInstanceOf(ShapeType.SHAPE item)
        {
            MyShape ms = ShapeFactory.ProduceShape(item);
            Prototypes.Add(item, ms);
        }

        // Hàm nhân bản hình
        public static MyShape CloneShape(ShapeType.SHAPE prams)
        {
            MyShape rs = null;
            switch (prams)
            { 
                case ShapeType.SHAPE.ARROW:
                    rs = new MyArrow();
                    rs.StartPoint = Prototypes[ShapeType.SHAPE.ARROW].StartPoint;
                    rs.EndPoint = Prototypes[ShapeType.SHAPE.ARROW].EndPoint;
                    rs.FillBrush = Prototypes[ShapeType.SHAPE.ARROW].FillBrush;
                    rs.StrokeBrush = Prototypes[ShapeType.SHAPE.ARROW].StrokeBrush;
                    rs.StrokeThickness = Prototypes[ShapeType.SHAPE.ARROW].StrokeThickness;
                    return rs;
            
                case ShapeType.SHAPE.ELLIPSE:
                    rs = new MyEllipse();
                    rs.StartPoint = Prototypes[ShapeType.SHAPE.ELLIPSE].StartPoint;
                    rs.EndPoint = Prototypes[ShapeType.SHAPE.ELLIPSE].EndPoint;
                    rs.FillBrush = Prototypes[ShapeType.SHAPE.ELLIPSE].FillBrush;
                    rs.StrokeBrush = Prototypes[ShapeType.SHAPE.ELLIPSE].StrokeBrush;
                    rs.StrokeThickness = Prototypes[ShapeType.SHAPE.ELLIPSE].StrokeThickness;
                    return rs;

                case ShapeType.SHAPE.HEART:
                    rs = new MyHeart();
                    rs.StartPoint = Prototypes[ShapeType.SHAPE.HEART].StartPoint;
                    rs.EndPoint = Prototypes[ShapeType.SHAPE.HEART].EndPoint;
                    rs.FillBrush = Prototypes[ShapeType.SHAPE.HEART].FillBrush;
                    rs.StrokeBrush = Prototypes[ShapeType.SHAPE.HEART].StrokeBrush;
                    rs.StrokeThickness = Prototypes[ShapeType.SHAPE.HEART].StrokeThickness;
                    return rs;

                case ShapeType.SHAPE.LINE:
                    rs = new MyLine();
                    rs.StartPoint = Prototypes[ShapeType.SHAPE.LINE].StartPoint;
                    rs.EndPoint = Prototypes[ShapeType.SHAPE.LINE].EndPoint;
                    rs.FillBrush = Prototypes[ShapeType.SHAPE.LINE].FillBrush;
                    rs.StrokeBrush = Prototypes[ShapeType.SHAPE.LINE].StrokeBrush;
                    rs.StrokeThickness = Prototypes[ShapeType.SHAPE.LINE].StrokeThickness;
                    return rs;

                case ShapeType.SHAPE.RECTANGLE:
                    rs = new MyRectangle();
                    rs.StartPoint = Prototypes[ShapeType.SHAPE.RECTANGLE].StartPoint;
                    rs.EndPoint = Prototypes[ShapeType.SHAPE.RECTANGLE].EndPoint;
                    rs.FillBrush = Prototypes[ShapeType.SHAPE.RECTANGLE].FillBrush;
                    rs.StrokeBrush = Prototypes[ShapeType.SHAPE.RECTANGLE].StrokeBrush;
                    rs.StrokeThickness = Prototypes[ShapeType.SHAPE.RECTANGLE].StrokeThickness;
                    return rs;

                case ShapeType.SHAPE.STAR:
                    rs = new MyStar();
                    rs.StartPoint = Prototypes[ShapeType.SHAPE.STAR].StartPoint;
                    rs.EndPoint = Prototypes[ShapeType.SHAPE.STAR].EndPoint;
                    rs.FillBrush = Prototypes[ShapeType.SHAPE.STAR].FillBrush;
                    rs.StrokeBrush = Prototypes[ShapeType.SHAPE.STAR].StrokeBrush;
                    rs.StrokeThickness = Prototypes[ShapeType.SHAPE.STAR].StrokeThickness;
                    return rs;

                case ShapeType.SHAPE.TRIANGLE:
                    rs = new MyTriangle();
                    rs.StartPoint = Prototypes[ShapeType.SHAPE.TRIANGLE].StartPoint;
                    rs.EndPoint = Prototypes[ShapeType.SHAPE.TRIANGLE].EndPoint;
                    rs.FillBrush = Prototypes[ShapeType.SHAPE.TRIANGLE].FillBrush;
                    rs.StrokeBrush = Prototypes[ShapeType.SHAPE.TRIANGLE].StrokeBrush;
                    rs.StrokeThickness = Prototypes[ShapeType.SHAPE.TRIANGLE].StrokeThickness;
                    return rs;

                default:
                    return null;
            }
            
        }
         
        // Hàm tạo và trả về đối tượng hình tùy thuộc kiểu SHAPE
        public static MyShape ProduceShape(ShapeType.SHAPE Sh)
        {
            
            MyShape result =  null;
            // Xác định đối tượng sẽ vẽ
            switch (Sh)
            {
                case ShapeType.SHAPE.LINE:
                    result = new MyLine();
                    break;

                case ShapeType.SHAPE.RECTANGLE:
                    result = new MyRectangle();
                    break;

                case ShapeType.SHAPE.ELLIPSE:
                    result = new MyEllipse();
                    break;

                case ShapeType.SHAPE.ARROW:
                    result = new MyArrow();
                    break;

                case ShapeType.SHAPE.TRIANGLE:
                    result = new MyTriangle();
                    break;

                case ShapeType.SHAPE.HEART:
                    result = new MyHeart();
                    break;

                case ShapeType.SHAPE.STAR:
                    result = new MyStar();
                    break;

                default:
                    break;
            }

            result.StartPoint = new Point(0, 0);
            result.EndPoint = new Point(0, 0);
            result.FillBrush = Brushes.Black;
            result.StrokeBrush = Brushes.Black;
            result.StrokeThickness = 3.0;
            
            return result;
        }
    }
}
