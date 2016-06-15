using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPaint
{
    

    public class ShapeFacTory
    {
         
        // Hàm tạo và trả về đối tượng hình tùy thuộc kiểu SHAPE
        public static MyShape ProduceShape(TypeShape.SHAPE Sh)
        {
            
            MyShape result =  null;
            // Xác định đối tượng sẽ vẽ
            switch (Sh)
            {
                case TypeShape.SHAPE.LINE:
                    result = new MyLine();
                    break;

                case TypeShape.SHAPE.RECTANGLE:
                    result = new MyRectangle();
                    break;

                case TypeShape.SHAPE.ELLIPSE:
                    result = new MyEllipse();
                    break;

                case TypeShape.SHAPE.ARROW:
                    result = new MyArrow();
                    break;

                case TypeShape.SHAPE.TRIANGLE:
                    result = new MyTriangle();
                    break;

                case TypeShape.SHAPE.HEART:
                    result = new MyHeart();
                    break;

                case TypeShape.SHAPE.STAR:
                    result = new MyStar();
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}
