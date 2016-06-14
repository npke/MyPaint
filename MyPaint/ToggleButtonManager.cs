using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
namespace MyPaint
{
    class ToggleButtonManager
    {
        private static List<ToggleButton> toggleBtnList = new List<ToggleButton>();

        public static void AddToggleButton(ToggleButton toggleButton)
        {
            toggleBtnList.Add(toggleButton);
        }

        public static void CheckButton(ToggleButton checkedBtn)
        {
            foreach (ToggleButton btn in toggleBtnList)
            {
                if (btn != checkedBtn)
                    btn.IsChecked = false;
            }
        }
    }
}
