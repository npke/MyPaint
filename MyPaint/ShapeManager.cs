using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint
{
    class ShapeManager
    {
        private static int nextHandle = 0;
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
