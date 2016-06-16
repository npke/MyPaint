using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPaint
{
    

    public class ShapeFactory
    {
         
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

            return result;
        }
    }
}
