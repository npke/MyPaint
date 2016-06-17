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
