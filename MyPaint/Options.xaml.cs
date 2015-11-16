using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyPaint
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public double RotateAngle = 0;

        public delegate void RotateCanvas(double angle);

        public RotateCanvas rotateCanvas;
        public Options()
        {
            InitializeComponent();
        }

        private void btnRotate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RotateAngle = Double.Parse(AngleCanvas.Text);
                if (rotateCanvas != null)
                    rotateCanvas.Invoke(RotateAngle);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
