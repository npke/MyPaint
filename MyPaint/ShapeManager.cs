using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPaintPlugin;

namespace MyPaint
{
    class ShapeManager
    {
        // Thuộc tính quy định mỗi Shape là có duy nhất một Handle.
        private static int nextHandle = 0;

        // Danh sách các Shape hiện hành của chương trình.
        private static Dictionary<int, MyShape> 
                        shapeList = new Dictionary<int, MyShape>();

        // Protoypes shape
        private static Dictionary<TypeShape.SHAPE, MyShape> Prototypes = new Dictionary<TypeShape.SHAPE, MyShape>();

        public static void PopulateAllShapeInstances()
        {
            var ListType =  Enum.GetValues(typeof(TypeShape.SHAPE));
            foreach (TypeShape.SHAPE item in ListType)
                CreateInstanceOf(item);
          
        }


        // Hàm tạo mới một shape và add vào danh sách prototype 
        private static void CreateInstanceOf(TypeShape.SHAPE item)
        {
            MyShape ms = ShapeFacTory.ProduceShape(item);
            Prototypes.Add(item, ms);
        }

        // Hàm nhân bản hình
        public static MyShape CloneShape(TypeShape.SHAPE prams)
        { 
            // Xem thuộc tính khởi tạo của đối tượng trong MainWindow để define clone
            return null;
        }
      


        public static int GetNextHandle()
        {
            return nextHandle++;
        }

        public static void AddShape(MyShape myShape)
        {
            shapeList.Add(myShape.Handle, myShape);
        }

        public static MyShape FindShapeByHanlde(int handle)
        {
            if (shapeList.Keys.Contains(handle))
                return shapeList[handle];
            return null;
        }
    }
}
