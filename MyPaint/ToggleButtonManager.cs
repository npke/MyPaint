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
        private List<ToggleButton> toggleBtnList = new List<ToggleButton>();

        public void AddToggleButton(ToggleButton toggleButton)
        {
            toggleBtnList.Add(toggleButton);
        }

        public void CheckButton(ToggleButton checkedBtn)
        {
            foreach (ToggleButton btn in toggleBtnList)
            {
                if (btn == null || btn != checkedBtn)
                    btn.IsChecked = false;
            }
        }
    }
}
