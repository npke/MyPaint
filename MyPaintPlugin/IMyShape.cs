using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyPaintPlugin
{
    public interface IMyShape
    {
        void Draw(System.Windows.Controls.UIElementCollection collection);
    }
}
