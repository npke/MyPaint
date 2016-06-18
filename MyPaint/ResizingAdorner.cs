using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Controls;
using System.Xml;
using System.Windows.Markup;
using System.IO;

namespace MyPaint
{
    // Nội dung phần code này xây dựng tham khảo từ mã nguồn và ý tưởng của tác giả DENYS VUIKA
    // Link: https://denisvuyka.wordpress.com/2007/10/13/the-wpf-resizing-adorner-for-canvas/
    public class ResizingAdorner : Adorner
    {
        // Tạo các thumb dùng như anchor point để thay đổi kích thước các hình vẽ
        Thumb topLeft, topRight, bottomLeft, bottomRight;
        Thumb middleTop, middleBottom, middleLeft, middleRight;

        // Tập chứa các thumb
        VisualCollection visualChildren;

        public Canvas drawingCanvas;

        // Phương thức khởi tạo
        public ResizingAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            visualChildren = new VisualCollection(this);

            // Khởi tạo các anchor point
            BuildAdorner(ref topLeft, Cursors.SizeNWSE);
            BuildAdorner(ref topRight, Cursors.SizeNESW);
            BuildAdorner(ref bottomLeft, Cursors.SizeNESW);
            BuildAdorner(ref bottomRight, Cursors.SizeNWSE);
            BuildAdorner(ref middleTop, Cursors.SizeNS);
            BuildAdorner(ref middleBottom, Cursors.SizeNS);
            BuildAdorner(ref middleLeft, Cursors.SizeWE);
            BuildAdorner(ref middleRight, Cursors.SizeWE);

            // Các sự kiện tương ứng với mỗi anchor point
            bottomLeft.DragDelta += new DragDeltaEventHandler(HandleBottomLeft);
            bottomRight.DragDelta += new DragDeltaEventHandler(HandleBottomRight);
            topLeft.DragDelta += new DragDeltaEventHandler(HandleTopLeft);
            topRight.DragDelta += new DragDeltaEventHandler(HandleTopRight);
            middleTop.DragDelta += new DragDeltaEventHandler(HandleMiddleTop);
            middleBottom.DragDelta += new DragDeltaEventHandler(HandleMiddleBottom);
            middleLeft.DragDelta += new DragDeltaEventHandler(HandleMiddleLeft);
            middleRight.DragDelta += new DragDeltaEventHandler(HandleMiddleRight);
        }

        // Xử lý khi resize với anchor point ở giữa bên trên
        void HandleMiddleTop(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            EnforceSize(adornedElement);

            double height_old = adornedElement.Height;
            double height_new = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);
            double top_old = Canvas.GetTop(adornedElement);
            adornedElement.Height = height_new;
            Canvas.SetTop(adornedElement, top_old - (height_new - height_old));
        }

        // Xử lý khi resize với anchor point ở giữa bên dưới
        void HandleMiddleBottom(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            EnforceSize(adornedElement);
            adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);
        }

        // Xử lý khi resize với anchor point ở giữa bên trái
        void HandleMiddleLeft(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            EnforceSize(adornedElement);

            double width_old = adornedElement.Width;
            double width_new = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            double left_old = Canvas.GetLeft(adornedElement);
            adornedElement.Width = width_new;

            Canvas.SetLeft(adornedElement, left_old - (width_new - width_old));
        }

        // Xử lý khi resize với anchor point ở giữa bên phải
        void HandleMiddleRight(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;
            EnforceSize(adornedElement);
            adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
        }

        // Xử lý khi resize với anchor point ở góc phải dưới
        void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            EnforceSize(adornedElement);

            adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);
        }

        // Xử lý khi resize với anchor point ở góc phải trên
        void HandleTopRight(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            EnforceSize(adornedElement);

            adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);

            double height_old = adornedElement.Height;
            double height_new = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);
            double top_old = Canvas.GetTop(adornedElement);
            adornedElement.Height = height_new;
            Canvas.SetTop(adornedElement, top_old - (height_new - height_old));
        }

        // Xử lý khi resize với anchor point ở góc trái trên trên
        void HandleTopLeft(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            EnforceSize(adornedElement);

            double width_old = adornedElement.Width;
            double width_new = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            double left_old = Canvas.GetLeft(adornedElement);
            adornedElement.Width = width_new;
            Canvas.SetLeft(adornedElement, left_old - (width_new - width_old));

            double height_old = adornedElement.Height;
            double height_new = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);
            double top_old = Canvas.GetTop(adornedElement);
            adornedElement.Height = height_new;
            Canvas.SetTop(adornedElement, top_old - (height_new - height_old));
        }

        // Xử lý khi resize với anchor point ở góc trái dưới
        void HandleBottomLeft(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            EnforceSize(adornedElement);

            adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);

            double width_old = adornedElement.Width;
            double width_new = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            double left_old = Canvas.GetLeft(adornedElement);
            adornedElement.Width = width_new;
            Canvas.SetLeft(adornedElement, left_old - (width_new - width_old));
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;
            double adornerWidth = this.DesiredSize.Width;
            double adornerHeight = this.DesiredSize.Height;

            topLeft.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            topRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            bottomLeft.Arrange(new Rect(-adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            bottomRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            middleTop.Arrange(new Rect(-adornerWidth / 200, -adornerHeight / 2, adornerWidth, adornerHeight));
            middleBottom.Arrange(new Rect(-adornerWidth / 200, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            middleLeft.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 200, adornerWidth, adornerHeight));
            middleRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 200, adornerWidth, adornerHeight));

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
            if (adornedElement.Width.Equals(Double.NaN))
                adornedElement.Width = adornedElement.DesiredSize.Width;
            if (adornedElement.Height.Equals(Double.NaN))
                adornedElement.Height = adornedElement.DesiredSize.Height;

            FrameworkElement parent = adornedElement.Parent as FrameworkElement;
            if (parent != null)
            {
                adornedElement.MaxHeight = parent.ActualHeight;
                adornedElement.MaxWidth = parent.ActualWidth;
            }
        }
        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            Rect selectionBox = new Rect
            {
                Width = this.AdornedElement.DesiredSize.Width,
                Height = this.AdornedElement.DesiredSize.Height,
            };

            Pen renderPen = new Pen();
            renderPen.Brush = Brushes.Black;
            renderPen.DashStyle = DashStyles.Dash;
            renderPen.Thickness = 2;

            drawingContext.DrawRectangle(null, renderPen, selectionBox);
        }

        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }
}
