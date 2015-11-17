using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyPaint
{
    class ResizingLineAdorner : Adorner
    {
        // Tạo các thumb dùng như anchor point để thay đổi kích thước các hình vẽ
        Thumb startThumb, endThumb;

        // Tập chứa các thumb
        VisualCollection visualChildren;

        // Phương thức khởi tạo
        public ResizingLineAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            visualChildren = new VisualCollection(this);

            // Khởi tạo các anchor point
            BuildAdorner(ref startThumb, Cursors.SizeNWSE);
            BuildAdorner(ref endThumb, Cursors.SizeNESW);
            

            // Các sự kiện tương ứng với mỗi anchor point
            startThumb.DragDelta += new DragDeltaEventHandler(HandleStartThumb);
            endThumb.DragDelta += new DragDeltaEventHandler(HandleEndThumb);
           
        }

        // Xử lý khi resize với anchor point ở giữa bên trên
        void HandleStartThumb(object sender, DragDeltaEventArgs args)
        {
            Line adornedElement = this.AdornedElement as Line;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            EnforceSize(adornedElement);

            Point position = Mouse.GetPosition(this);

            adornedElement.X1 = position.X;
            adornedElement.Y1 = position.Y;
            InvalidateArrange();
        }

        // Xử lý khi resize với anchor point ở giữa bên dưới
        void HandleEndThumb(object sender, DragDeltaEventArgs args)
        {
            Line adornedElement = this.AdornedElement as Line;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            EnforceSize(adornedElement);

            Point position = Mouse.GetPosition(this);

            adornedElement.X2 = position.X;
            adornedElement.Y2 = position.Y;
            InvalidateArrange();
        }

        // Một vài xử lý được thảm khảo từ Stack Over Flow
        // Link: http://stackoverflow.com/questions/17568796/how-to-arrange-thumbs-with-a-line-for-moving-a-line-in-a-wpf-custom-adorner
        protected override Size ArrangeOverride(Size finalSize)
        {
            Line selectedLine = AdornedElement as Line;

            double left = Math.Min(selectedLine.X1, selectedLine.X2);
            double top = Math.Min(selectedLine.Y1, selectedLine.Y2);

            var startRect = new Rect(selectedLine.X1 - (startThumb.Width / 2), selectedLine.Y1 - (startThumb.Width / 2), startThumb.Width, startThumb.Height);
            startThumb.Arrange(startRect);

            var endRect = new Rect(selectedLine.X2 - (endThumb.Width / 2), selectedLine.Y2 - (endThumb.Height / 2), endThumb.Width, endThumb.Height);
            endThumb.Arrange(endRect);

            return finalSize;
        }

        // Thiết lập các thumb
        void BuildAdorner(ref Thumb cornerThumb, Cursor customizedCursor)
        {
            if (cornerThumb != null) return;

            cornerThumb = new Thumb();

            cornerThumb.Cursor = customizedCursor;
            cornerThumb.Height = cornerThumb.Width = 8;
            cornerThumb.Height = cornerThumb.Height = 8;
            cornerThumb.BorderBrush = new SolidColorBrush(Colors.Black);
            cornerThumb.BorderThickness = new Thickness(0.8);
            cornerThumb.Foreground = new SolidColorBrush(Colors.White);
            cornerThumb.Background = new SolidColorBrush(Colors.White);

            visualChildren.Add(cornerThumb);
        }

        void EnforceSize(FrameworkElement adornedElement)
        {

            FrameworkElement parent = adornedElement.Parent as FrameworkElement;
            if (parent != null)
            {
                Line selectedLine = AdornedElement as Line;
                double left = Math.Min(selectedLine.X1, selectedLine.X2);
                double top = Math.Min(selectedLine.Y1, selectedLine.Y2);
                adornedElement.MaxHeight = parent.ActualHeight + top;
                adornedElement.MaxWidth = parent.ActualWidth + left;
            }
        }

        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }
}
