using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private static Dictionary<ShapeType.SHAPE, MyShape> Prototypes = new Dictionary<ShapeType.SHAPE, MyShape>();

        public static void PopulateAllShapeInstances()
        {
            var ListType =  Enum.GetValues(typeof(ShapeType.SHAPE));
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
