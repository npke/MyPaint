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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Ribbon;
using System.IO.Packaging;
using System.IO;
using System.Windows.Markup;
using Microsoft.Win32;
using System.Xml;
using System.Windows.Controls.Primitives;

namespace MyPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Hình sẽ vẽ
        MyShape myShape;
       
        // Các đối tượng là tham số đầu vào khi vẽ một hình
        Point startPoint, endPoint;
        Brush strokeBrush, fillBrush, textBrush;
        double strokeThickness, top, left;
        DoubleCollection dashCollection = new DoubleCollection { };

        // Các biến đánh dấu trạng thái con trở chuột
        bool isMouseDowned = false;
        bool isDragging = false;
        bool isMoved = false;

        // Kiểu enum xác định hình sẽ vẽ và chế độ
        enum SHAPE { LINE = 1, RECTANGLE = 2, ELLIPSE = 3 };
        enum MODE { DRAW = 1, SELECT = 2 }

        SHAPE drawWhat = SHAPE.LINE;
        MODE drawMode = MODE.DRAW;

        public static Shape selectedShape = null;
        public static TextBox selectedTextBox = null;
        public static UIElement selectedUIElement = null;

        public static AdornerLayer adornerLayer;

        // Các stack phục vụ mục đích undo và redo
        public static Stack<List<UIElement>> UndoStack = new Stack<List<UIElement>>();
        public static Stack<List<UIElement>> RedoStack = new Stack<List<UIElement>>();

        public static Canvas ijk;
        

        // Đếm số lần thực hiện paste một đối tượng
        int iPaste = 0;
        double iLeft = 0, iTop = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeShape();
        }

        // Thiết lập các thuộc tính mặc định cho hình vẽ
        private void InitializeShape()
        {
            startPoint = new Point(0, 0);
            endPoint = new Point(0, 0);

            strokeBrush = Brushes.Black;
            fillBrush = Brushes.Transparent;
            textBrush = Brushes.Black;

            strokeThickness = 3;
        }


		// Chế độ chọn
        private void setDrawMode(object sender, RoutedEventArgs e)
        {
            if (drawMode == MODE.SELECT)
            {
                drawMode = MODE.DRAW;
            }
            else
            {
                drawMode = MODE.SELECT;
                tglbtnEllipse.IsChecked = false;
                tglbtnRectangle.IsChecked = false;
                tglbtnText.IsChecked = false;
                tglbtnImage.IsChecked = false;
                tglbtnLine.IsChecked = false;
            }
        }


        // Đánh dấu đối tượng sẽ vẽ là đường thằng
        private void drawLine(object sender, RoutedEventArgs e)
        {
            RemoveAdorner();
            drawWhat = SHAPE.LINE;
            tglbtnEllipse.IsChecked = false;
            tglbtnRectangle.IsChecked = false;
            tglbtnText.IsChecked = false;
            tglbtnImage.IsChecked = false;
            tglbtnSelect.IsChecked = false;
            if (drawMode == MODE.SELECT)
                drawMode = MODE.DRAW;
        }

        // Đánh dấu đối tượng sẽ vẽ là hình chữ nhật
        private void drawRectangle(object sender, RoutedEventArgs e)
        {
            RemoveAdorner();
            drawWhat = SHAPE.RECTANGLE;
            tglbtnEllipse.IsChecked = false;
            tglbtnLine.IsChecked = false;
            tglbtnText.IsChecked = false;
            tglbtnImage.IsChecked = false;
            tglbtnSelect.IsChecked = false;
            if (drawMode == MODE.SELECT)
                drawMode = MODE.DRAW;
        }

        // Đánh dấu đối tượng sẽ vẽ là hình e líp
        private void drawEllipse(object sender, RoutedEventArgs e)
        {
            RemoveAdorner();
            drawWhat = SHAPE.ELLIPSE;
            tglbtnRectangle.IsChecked = false;
            tglbtnLine.IsChecked = false;
            tglbtnText.IsChecked = false;
            tglbtnImage.IsChecked = false;
            tglbtnSelect.IsChecked = false;
            if (drawMode == MODE.SELECT)
                drawMode = MODE.DRAW;
        }

        // Xử lý khi bấm chuột trên canvas
        private void drawingCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Xử lý cho thumb resize canvas
            if (e.Source == canvasResizerRightBottom)
            {
                return;
            }

            // Lấy vị trí con trỏ chuột bấm xuống và đánh dấu trạng thái chuột là đã bấm
            startPoint = e.GetPosition(drawingCanvas);
            isMouseDowned = true;

            // Nếu double click thì tạo textbox tại vị trí vừa click
            if (e.ClickCount == 2 && e.Source == drawingCanvas)
            {
                Point position = e.GetPosition(drawingCanvas);
                addText(position.Y, position.X);
                isMouseDowned = false;
            }

            // Nếu đang ở chế độ vẽ
            // Kiểm tra xem có phải click chuột lên đối tượng vừa vẽ hay không
            // Đúng: => Cho phép tiếp tục resize
            // Sai: => Đánh dấu chuẩn bị vẽ đối tượng mới

            if (drawMode == MODE.DRAW) // Chế độ vẽ
            {
                // Nếu vừa vẽ một đối tượng và bấm lên chính đối tượng đó
                if (selectedUIElement != null && e.Source == selectedUIElement)
                {
                    // Xác định khoảng cách tới biên trên và biên trái của đối tượng trong canvas
                    if (Canvas.GetTop(selectedUIElement).Equals(Double.NaN))
                        top = 0;
                    else top = Canvas.GetTop(selectedUIElement);

                    if (Canvas.GetLeft(selectedUIElement).Equals(Double.NaN))
                        left = 0;
                    else left = Canvas.GetLeft(selectedUIElement);

                    // Chuyển con trỏ chuột sang trạng thái SizeAll
                    this.Cursor = Cursors.SizeAll;
                    e.Handled = true;
                    return;
                }

                // Nếu có đối tượng vừa vẽ mà lại bấm lên canvas hoặc đối tượng khác
                // => Remove lớp Adorner của đối tượng vừa vẽ chuẩn bị vẽ đối tượng mới
                if (selectedUIElement != null && e.Source != selectedUIElement  && selectedTextBox == null)
                {
                    // Remove Adorner layer
                    RemoveAdorner();
                }

                // Xác định đối tượng sẽ vẽ
                switch (drawWhat)
                {
                    case SHAPE.LINE:
                        if (myShape == null)
                            myShape = new MyLine();
                        break;

                    case SHAPE.RECTANGLE:
                        if (myShape == null)
                            myShape = new MyRectangle();
                        break;

                    case SHAPE.ELLIPSE:
                        if (myShape == null)
                            myShape = new MyEllipse();
                        break;

                    default:
                        break;
                }
            }

            // Nếu ở chế độ chọn
            // Nếu click lên một đối tượng thì cho phép move, resize đối tượng đó
            if (drawMode == MODE.SELECT)
            {

                // Nếu bấm lên một đối tượng khác đối tượng vừa chọn trước đó
                // => Remove adorner layer ở đối tượng cũ
                if (e.Source != selectedUIElement && selectedUIElement != null)
                {
                    RemoveAdorner();
                }

                // Nếu bấm chuột lên một đối tượng khác
                if (e.Source != drawingCanvas)
                {
                    // Lấy đối tượng đã chọn
                    selectedUIElement = e.Source as UIElement;
                    if (selectedUIElement is Shape)
                    {
                        selectedShape = e.Source as Shape;
                    }
                    else if (selectedUIElement is TextBox)
                    {
                        selectedTextBox = e.Source as TextBox;
                        Keyboard.Focus(selectedTextBox);
                    }
                    else
                    {

                    }

                    // Xác định khoảng cách tới biên trên và biên trái của đối tượng trong canvas
                    if (Canvas.GetTop(selectedUIElement).Equals(Double.NaN))
                        top = 0;
                    else top = Canvas.GetTop(selectedUIElement);

                    if (Canvas.GetLeft(selectedUIElement).Equals(Double.NaN))
                        left = 0;
                    else left = Canvas.GetLeft(selectedUIElement);

                    // Tạo một AdornerLayer cho đối tượng và thêm vào adornerLayer
                    AddAdorner(selectedUIElement);

                    // Chuyển con trỏ chuột sang trạng thái SizeAll
                    this.Cursor = Cursors.SizeAll;
                }
            }
            e.Handled = true;
        }

        // Xử lý khi di chuyển chuột trong Canvas
        private void drawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Nếu không phải đang bấm chuột thì không làm gì cả
            if (!isMouseDowned)
                return;

            // Nếu đang ở chế độ vẽ => Vẽ đối tượng
            if (drawMode == MODE.DRAW && selectedUIElement == null)
            {
                // Nhả chuột
                if (e.LeftButton == MouseButtonState.Released)
                    return;

                if (!((Math.Abs(e.GetPosition(drawingCanvas).X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                        (Math.Abs(e.GetPosition(drawingCanvas).Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    return;

                // Lấy ví trị chuột mỗi khi di chuyển
                endPoint = e.GetPosition(drawingCanvas);

                // Kiểm tra phím Shift có đang được bấm, dùng khi vẽ hình vuông & hình tròn
                bool shiftKeyPressed = false;
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    shiftKeyPressed = true;

                // ??? Chuyển thành hàm, chỉnh lại thông số
                // Thiết lập các thuộc tính cho đối tượng cần vẽ
                if (startPoint.Equals(endPoint))
                    return;
                myShape.StrokeBrush = strokeBrush;
                myShape.FillBrush = fillBrush;
                myShape.StrokeThickness = strokeThickness;
                myShape.StartPoint = startPoint;
                myShape.EndPoint = endPoint;
                myShape.DashCollection = dashCollection;

                // Gọi hàm vẽ đối tượng tương ứng, đây là đối tượng xem trước
                myShape.DrawOnMouseMove(drawingCanvas.Children, shiftKeyPressed);
            }

            // Nếu ở chế độ chọn => cho phép di chuyển, thay đổi kích thước
            if ((drawMode == MODE.SELECT && selectedUIElement != null) || selectedUIElement != null)
            {
                // Kiểm tra phím Ctr có đang được bấm thì sẽ thực hiện xoay đối tượng
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    // Xác định các điểm tâm và điểm tạo góc
                    Point centerPoint = new Point();
                    endPoint = e.GetPosition(drawingCanvas);

                    // Không cho phép xoay đường thẳng
                    if (selectedUIElement is Line)
                        return;

                    // Xoay các hình
                    if (selectedUIElement is Shape)
                    {
                        centerPoint.X = selectedShape.Width / 2 + left;
                        centerPoint.Y = selectedShape.Height / 2 + top;
                    }
                    else if (selectedUIElement is TextBox) // Xoay văn bản
                    {
                        centerPoint.X = left + (selectedTextBox.ActualWidth / 2);
                        centerPoint.Y = top + (selectedTextBox.ActualHeight / 2);
                    }

                    // Hai vector tạo góc xoay
                    Vector startVector = Point.Subtract(startPoint, centerPoint);
                    Vector deltaVector = Point.Subtract(endPoint, centerPoint);

                    // Góc giữa hai vector
                    double angle = Vector.AngleBetween(startVector, deltaVector);

                    // Xoay đối tượng
                    selectedUIElement.RenderTransform = new RotateTransform(angle, centerPoint.X - left, centerPoint.Y - top);
                    isMoved = true;
                    return;
                }

                // Kiểm tra có phải người dùng đang thực hiện thao tác Drag
                if ( (isDragging == false) &&
                    ( (Math.Abs(e.GetPosition(drawingCanvas).X -  startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) || 
                        (Math.Abs(e.GetPosition(drawingCanvas).Y -  startPoint.Y) > SystemParameters.MinimumVerticalDragDistance) ) )
                {
                    isDragging = true;
                }

                // Thực hiện di chuyển đối tượng
                if (isDragging)
                {
                    Point currentPoint = e.GetPosition(drawingCanvas);

                    Canvas.SetTop(selectedUIElement, currentPoint.Y - (startPoint.Y - top));
                    Canvas.SetLeft(selectedUIElement, currentPoint.X - (startPoint.X - left));
                    isMoved = true;
                }
            }
        }

		
		// Phương thức lưu lại các đối tượng hiện có của Canvas vào trong stack
        private void SnapCanvas()
        {
            List<UIElement> UIEList = new List<UIElement>();
            foreach (UIElement UIE in drawingCanvas.Children)
            {
                UIElement newUIE = DeepClone(UIE);
                UIEList.Add(newUIE);
            }

            UndoStack.Push(UIEList);
        }

		// Phương thức sao chép một đối tượng theo kiểu deep
        private UIElement DeepClone(UIElement element)
        {
            string shapeStr = XamlWriter.Save(element);
            StringReader strReader = new StringReader(shapeStr);
            XmlTextReader xmlTextReader = new XmlTextReader(strReader);
            UIElement CopyObject = (UIElement)XamlReader.Load(xmlTextReader);
            return CopyObject;
        }

        // Xử lý sự kiện khi nhả chuột
        private void drawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDowned = false;

            if (isMoved)
            {
                SnapCanvas();
            }

        	// Nếu ở chế độ vẽ
            if (drawMode == MODE.DRAW)
            {
                endPoint = e.GetPosition(drawingCanvas);
                bool shiftKeyPressed = false;

                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    shiftKeyPressed = true;

                // Vẽ đối tượng với các thông số cuối cùng lên canvas
                if (selectedUIElement == null)
                {
                    if (myShape == null)
                        return;

                    try
                    {
                        myShape.DrawOnMouseUp(drawingCanvas.Children, shiftKeyPressed);
                        if (myShape.DrawedElement == null)
                            return;

                        AddAdorner(myShape.DrawedElement);
                        SnapCanvas();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "1");
                    }
                }
                myShape = null;
            }
            
            // Nếu ở chế độ chọn
            if (drawMode == MODE.SELECT || selectedUIElement != null)
            {
            	// Bỏ đánh dấu chuột đang bấm xuống và đang thực hiện Drag
                if (isMouseDowned)
                {
                    isDragging = false;
                }
            }

            // Đưa con trỏ chuột về hình dạng bình thường
            this.Cursor = Cursors.Arrow;
        }

        // Xử lý phím bấm 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
				// Xử lý thao tác xóa
                if (selectedUIElement != null)
                {
                    SnapCanvas();
                    drawingCanvas.Children.Remove(selectedUIElement);
                }

                selectedUIElement = null;
                selectedShape = null;
                selectedTextBox = null;
            }
			
			// Bấm - để giảm ZIndex của một đối tượng trong canvas
            if (e.Key == Key.Subtract && selectedUIElement != null)
            {
                Canvas.SetZIndex(selectedUIElement, Canvas.GetZIndex(selectedUIElement) - 1);
            }
			
			// Bấm + để tăng ZIndex của một đối tượng trong canvas
            if (e.Key == Key.Add && selectedUIElement != null)
            {
                Canvas.SetZIndex(selectedUIElement, Canvas.GetZIndex(selectedUIElement) + 1);
            }
        }

        // Phương thức cho phép người dùng chọn một màu từ bảng màu
        private SolidColorBrush editColor()
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.FullOpen = true;
            colorDialog.ShowDialog();

            System.Windows.Media.Color color = new System.Windows.Media.Color();
            color.A = colorDialog.Color.A;
            color.B = colorDialog.Color.B;
            color.G = colorDialog.Color.G;
            color.R = colorDialog.Color.R;

            SolidColorBrush colorBrush = new SolidColorBrush();
            colorBrush.Color = color;

            return colorBrush;
        }

        // Đổi màu đường viền nét vẽ
        private void editStrokeColor(object sender, RoutedEventArgs e)
        {
            strokeBrush = editColor();
            StrokeBrush.Fill = strokeBrush;

            if (selectedShape != null)
                selectedShape.Stroke = strokeBrush;
            if (selectedTextBox != null)
                selectedTextBox.BorderBrush = strokeBrush;

            if (selectedUIElement != null)
            {
                SnapCanvas();
            }
        }

        // Đổi màu nền đối tượng vẽ
        private void editFillColor(object sender, RoutedEventArgs e)
        {
            fillBrush = editColor();
            FillBrush.Fill = fillBrush;

            if (selectedShape != null)
                selectedShape.Fill = fillBrush;
            if (selectedTextBox != null)
                selectedTextBox.Background = fillBrush;

            if (selectedUIElement != null)
            {
                SnapCanvas();
            }
        }

		
		// Phương thức xóa bỏ hết các Adorner hiện có
        public static void RemoveAdorner()
        {
            if (selectedUIElement == null)
                return;

            Adorner[] listAdorner = adornerLayer.GetAdorners(selectedUIElement);
            if (listAdorner == null)
                return;
				
            for (int i = 0; i < listAdorner.Count(); i++)
                adornerLayer.Remove(listAdorner[i]);

            selectedShape = null;
            selectedUIElement = null;
            selectedTextBox = null;
        }

		// Phương thức thêm Adorner vào một UIELement
        public static void AddAdorner(UIElement uiElement)
        {
            if (uiElement == null)
                return;

            adornerLayer = AdornerLayer.GetAdornerLayer(uiElement);
            if (adornerLayer == null)
                return;

            if (uiElement is Line)
            {
                adornerLayer.Add(new ResizingLineAdorner(uiElement));
            }
            else
            {
                adornerLayer.Add(new ResizingAdorner(uiElement));
            }

            selectedUIElement = uiElement;
            if (uiElement is Shape)
            {
                selectedShape = (Shape)uiElement;
            }
            else if (uiElement is TextBox)
            {
                selectedTextBox = (TextBox)uiElement;
            }
        }
		
		// Phương thức thêm văn bản vào canvas
        private void addText(double Top, double Left)
        {
            tglbtnEllipse.IsChecked = false;
            tglbtnLine.IsChecked = false;
            tglbtnImage.IsChecked = false;
            tglbtnRectangle.IsChecked = false;

            drawMode = MODE.SELECT;
            tglbtnSelect.IsChecked = true;
            
            RemoveAdorner();

			// Tạo đối tượng textbox và thiết lập các thuộc tính
            TextBox textBox = new TextBox();
            textBox.Focusable = true;
            textBox.TextAlignment = TextAlignment.Center;
            textBox.Width = 200;
            textBox.Height = 50;
            textBox.BorderBrush = strokeBrush;
            textBox.BorderThickness = new Thickness(3);
			textBox.Foreground = textBrush;
            textBox.Background = fillBrush;
            textBox.TextAlignment = TextAlignment.Center;
            textBox.FontSize = 12;
            textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.AcceptsReturn = true;
			
			// Binding font chữ
            if (FontList.SelectedIndex < 0)
                FontList.SelectedIndex = 0;

            Binding myBinding = new Binding();
            myBinding.Source = FontList;

            BindingOperations.SetBinding(textBox, TextBlock.FontFamilyProperty, myBinding);
            string selectedFont = FontList.SelectedItem.ToString();
			
            if (selectedFont != "")
                textBox.FontFamily = new FontFamily(selectedFont);
			
			// Bold, Italic, Underline
            if (tglbtnBold.IsChecked == true)
            {
                textBox.FontWeight = FontWeights.Bold;
            }

            if (tglbtnItalic.IsChecked == true)
            {
                textBox.FontStyle = FontStyles.Italic;
            }

            if (tglbtnUnderline.IsChecked == true)
            {
                textBox.TextDecorations = TextDecorations.Underline;
            }

			// Font chữ
            try
            {
                int fontSize = int.Parse(txtFontSize.Text);
                textBox.FontSize = fontSize;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            drawingCanvas.Children.Add(textBox);
            AddAdorner(textBox);

            Canvas.SetTop(textBox, Top - textBox.Height / 2);
            Canvas.SetLeft(textBox, Left - textBox.Width / 2);
            
            Keyboard.Focus(selectedTextBox);
        }

		// Kiểm tra có thực hiện được thao tác copy hay không
        private void CopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (selectedShape != null)
                e.CanExecute = true;
            else 
                e.CanExecute = false;
        }

		// Thực hiện copy
        private void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string xaml = XamlWriter.Save(selectedUIElement);
            Clipboard.SetText(xaml, TextDataFormat.Xaml);
			
            iPaste = 1; // Số lần paste dùng để xác định vị trí cho các đối tượng khi paste
            iLeft = Canvas.GetLeft(selectedUIElement);
            iTop = Canvas.GetTop(selectedUIElement);
        }

		// Kiểm tra có thực hiện được thao tác cut hay không
        private void CutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (selectedShape != null)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

		// Thực hiện thao tác cut
        private void CutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string xaml = XamlWriter.Save(selectedUIElement);
            Clipboard.SetText(xaml, TextDataFormat.Xaml);
            drawingCanvas.Children.Remove(selectedUIElement);

            iPaste = 1;
            iLeft = Canvas.GetLeft(selectedUIElement);
            iTop = Canvas.GetTop(selectedUIElement);

            selectedShape = null;
            selectedUIElement = null;
			selectedTextBox = null;
        }

		// Kiểm tra trước khi thực hiện thao tác paste
        private void PasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsText(TextDataFormat.Xaml);
        }

		// Thực hiện paste 
        private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string xaml = Clipboard.GetText(TextDataFormat.Xaml);
            if (xaml != null)
            {
                using (MemoryStream stream = new MemoryStream(xaml.Length))
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(xaml);
                        sw.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        Shape shape = XamlReader.Load(stream) as Shape;

                        if (shape is Line)
                        {
                            Line line = new Line();
                            line = (Line)shape;
                            line.X1 += (10 + line.StrokeThickness) * iPaste;
                            line.X2 += (10 + line.StrokeThickness) * iPaste;
                        }
                        else
                        {
                            Canvas.SetLeft(shape, iLeft + (10 + shape.StrokeThickness) * iPaste);
                            Canvas.SetTop(shape, iTop + (10 + shape.StrokeThickness) * iPaste);
                        }

                        drawingCanvas.Children.Add(shape);
                        SnapCanvas();
                        RemoveAdorner();
                        AddAdorner(shape);
                        iPaste++;
                    }
               }
            }
        }

		// Kiểm tra có thực hiện được thao tác undo hay không
        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (UndoStack.Count() > 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

		// Xử lý undo
        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			// Lấy trạng thái mới nhất bỏ vào RedoStack
            List<UIElement> undoList1 = UndoStack.Pop();
            RedoStack.Push(undoList1);
            
			// Nếu trước đó chỉ có 1 trạng thái => Dừng 
            drawingCanvas.Children.Clear();
            if (UndoStack.Count == 0)
            {
                RemoveAdorner();
                return;
            }

			// Lấy trạng thái trước đó và đưa canvas về trạng thái này
            List<UIElement> undoList2 = UndoStack.Pop();
            foreach (UIElement UIE in undoList2)
            {
                drawingCanvas.Children.Add(UIE);
            }
			
			// Thiết lập selectedUIElement = null
            if (selectedUIElement != null)
            {
                selectedShape = null;
                selectedUIElement = null;
                selectedTextBox = null;
            }
			
			// Đưa lại trạng thái vào stack
            UndoStack.Push(undoList2);
        }

		// Kiểm tra có thực hiện được thao tác redo hay không
        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (RedoStack.Count() > 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

		// Thực hiện xử lý redo
        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
			// Lấy trạng thái canvas trong redo stack
            List<UIElement> redoList = RedoStack.Pop();
            UndoStack.Push(redoList);
			
			// Thực hiện khôi phục lại trạng thái đó
            drawingCanvas.Children.Clear();
            foreach (UIElement UIE in redoList)
            {
                drawingCanvas.Children.Add(UIE);
            }
        }

		// Phương thức thêm hình ảnh vào canvas 
		// Chưa xử lý xong
        private void addImage(object sender, RoutedEventArgs e)
        {
            tglbtnEllipse.IsChecked = false;
            tglbtnLine.IsChecked = false;
            tglbtnText.IsChecked = false;
            tglbtnRectangle.IsChecked = false;
            tglbtnSelect.IsChecked = false;
            if (drawMode == MODE.SELECT)
                drawMode = MODE.DRAW;
        }

		// Thực hiện cập nhật khi thuộc tính StrokeThickness thay đổi
        private void cbStrokeThickness_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cbStrokeThickness.SelectedIndex;
            string thickness = cbStrokeThickness.Items[index].ToString();

            if (thickness.Length == 41)
                thickness = thickness.Substring(38, 1);
            else
                thickness = thickness.Substring(38, 2);
           
            if (selectedShape != null)
            {
                selectedShape.StrokeThickness = int.Parse(thickness);
            }

            if (selectedTextBox != null)
            {
                selectedTextBox.BorderThickness = new Thickness(int.Parse(thickness));
            }

            strokeThickness = int.Parse(thickness);

            if (selectedUIElement != null)
            {
                SnapCanvas();
            }
        }

		// Thực hiện cập nhật khi thuộc tính StrokeStyle thay đổi
        private void cbStrokeStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cbStrokeStyle.SelectedIndex;
            switch (index)
            {
                case 0:
                    dashCollection = new DoubleCollection { };
                    break;

                case 1:
                    dashCollection = new DoubleCollection { 1, 2 };
                    break;

                case 2:
                    dashCollection = new DoubleCollection { 3, 2 };
                    break;

                case 3:
                    dashCollection = new DoubleCollection { 3, 2, 1, 2 };
                    break;

                case 4:
                    dashCollection = new DoubleCollection { 5, 2 };
                    break;

                case 5:
                    dashCollection = new DoubleCollection { 5, 2, 1, 2 };
                    break;
            }

            if (selectedShape != null)
            {
                selectedShape.StrokeDashArray = dashCollection;
                SnapCanvas();
            }
        }

		// Thực hiện cập nhật khi font thay đổi
        private void FontList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                string selectedFont = FontList.SelectedItem.ToString();
                if (selectedFont != "")
                    selectedTextBox.FontFamily = new FontFamily(selectedFont);
                SnapCanvas();
            }
        }


		// Xử lý khi người dùng nhập vào textBox giá trị fontsize
        private void txtFontSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (selectedTextBox != null)
            {
                try
                {
                    int fontSize = int.Parse(txtFontSize.Text);
                    selectedTextBox.FontSize = fontSize;
                    SnapCanvas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

		// Thực hiện cập nhật khi thuộc tính Bold thay đổi
        private void tglbtnBold_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.FontWeight = FontWeights.Bold;
                SnapCanvas();  
            }
        }

		// Thực hiện cập nhật khi thuộc tính Italic thay đổi
        private void tglbtnItalic_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.FontStyle = FontStyles.Italic;
                SnapCanvas();
            }
        }

		// Thực hiện cập nhật khi thuộc tính Underline thay đổi
        private void tglbtnUnderline_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.TextDecorations = TextDecorations.Underline;
                SnapCanvas();
            }
        }
		
		// Thực hiện cập nhật khi thuộc tính Bold thay đổi
        private void tglbtnBold_Unchecked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.FontWeight = FontWeights.Normal;
                SnapCanvas();
            }
        }

		// Thực hiện cập nhật khi thuộc tính Italic thay đổi
        private void tglbtnItalic_Unchecked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.FontStyle = FontStyles.Normal;
                SnapCanvas();
            }
        }

		// Thực hiện cập nhật khi thuộc tính Underline thay đổi
        private void tglbtnUnderline_Unchecked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.TextDecorations = null;
                SnapCanvas();
            }
        }

		// Xử lý khi người dúng bấm nút xóa
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUIElement != null)
            {
                SnapCanvas();
                drawingCanvas.Children.Remove(selectedUIElement);
            }

            selectedUIElement = null;
            selectedShape = null;
        }
	
		// Căn chỉnh Horozontal Aligment của văn bản
        private void HAlignment_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox == null)
                return;

            MenuItem menuItem = sender as MenuItem;
            switch (menuItem.Header.ToString())
            {
                case "Left":
                    selectedTextBox.TextAlignment = TextAlignment.Left;
                    break;

                case "Right":
                    selectedTextBox.TextAlignment = TextAlignment.Right;
                    break;

                case "Center":
                    selectedTextBox.TextAlignment = TextAlignment.Center;
                    break;

                case "Justify":
                    selectedTextBox.TextAlignment = TextAlignment.Justify;
                    break;
            }
        }

		// Căn chỉnh Vertical Aligment của văn bản
        private void VAlignment_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox == null)
                return;

            MenuItem menuItem = sender as MenuItem;
            switch (menuItem.Header.ToString())
            {
                case "Top":
                    selectedTextBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
                    break;

                case "Bottom":
                    selectedTextBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Bottom;
                    break;

                case "Center":
                    selectedTextBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                    break;
            }
        }
		
		// Thiết lập background canvas là trong suốt
        private void mnItmTransparent_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.Background = Brushes.Transparent;
            mnItmFill.IsChecked = false;
        }
		
		// Thiết lập background canvas là một màu solid
        private void mnItmFill_Click(object sender, RoutedEventArgs e)
        {
            drawingCanvas.Background = editColor();
            mnItmFill.IsChecked = true;
            mnItmTransparent.IsChecked = false;
        }

		// Thay đổi màu chữ
        private void editTextColor(object sender, RoutedEventArgs e)
        {
            textBrush = editColor();
            TextColor.Fill = textBrush;
            if (selectedTextBox != null)
            {
                selectedTextBox.Foreground = textBrush;
                SnapCanvas();
            }
        }
		
		// Xóa bỏ background văn bản
        private void RemoveTextBackground(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.Background = Brushes.Transparent;
                SnapCanvas();
            }
        }
		
		// Xóa bỏ viền văn bản
        private void RemoveTextBorder(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.BorderBrush = Brushes.Transparent;
                SnapCanvas();
            }
        }
		
		
		// Xử lý save command
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Size size = new Size(drawingCanvas.ActualWidth, drawingCanvas.ActualHeight);

            drawingCanvas.Measure(size);
            drawingCanvas.Arrange(new Rect(size));

            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                System.Windows.Media.PixelFormats.Default);

            renderBitmap.Render(drawingCanvas);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Untitled";
            saveFileDialog.DefaultExt = ".paint";
            saveFileDialog.Filter = " Paint (*.paint)|*.paint|Bitmap (*.bmp)|*.bmp|PNG (*.png)|*.png|GIF (*.gif)|*.gif|JPEG (*.jpg)|*.jpg";

            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
				BitmapEncoder bitmapEncoder;
				 
                switch (System.IO.Path.GetExtension(saveFileDialog.FileName))
                {
                    case ".bmp":
                        bitmapEncoder = new BmpBitmapEncoder();
                        break;

                    case ".png":
                        bitmapEncoder = new PngBitmapEncoder();
                        break;

                    case ".gif":
                        bitmapEncoder = new GifBitmapEncoder();
                        break;

                    case ".jpg":
                        bitmapEncoder = new JpegBitmapEncoder();
                        break;

                    default:
                        string[] xamlShape = new string[drawingCanvas.Children.Count];
                        int i = 0;
                        foreach (Shape mySaveShape in drawingCanvas.Children)
                        {
                            xamlShape[i] = XamlWriter.Save(mySaveShape);
                            i++;
                        }
                        System.IO.File.WriteAllLines(saveFileDialog.FileName, xamlShape);
                        ms.Close();
                        this.Title = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + "- My Paint";
                        return;
                }
				
				bitmapEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                bitmapEncoder.Save(ms);
				
                ms.Close();
                System.IO.File.WriteAllBytes(saveFileDialog.FileName, ms.ToArray());
                this.Title = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + "- My Paint";
            }
        }

		// Xử lý open command
        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (drawingCanvas.Children.Count != 0)
            {
                MessageBoxResult result = MessageBox.Show("Close current paint and open new?", "My Paint", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result != MessageBoxResult.OK)
                {
                    return;
                }
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = " Paint (*.paint)|*.paint|Bitmap (*.bmp)|*.bmp|PNG (*.png)|*.png|GIF (*.gif)|*.gif|JPEG (*.jpeg)|*.jpg";

            if (openFileDialog.ShowDialog() == false)
                return;
            this.Title = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName) + " - My Paint";
            if (System.IO.Path.GetExtension(openFileDialog.FileName) == ".paint")
            {
                drawingCanvas.Children.Clear();
                string[] xamlShape = System.IO.File.ReadAllLines(openFileDialog.FileName);
                foreach (string xamlShapeString in xamlShape)
                {
                    if (xamlShapeString != null)
                    {
                        using (MemoryStream stream = new MemoryStream(xamlShapeString.Length))
                        {
                            using (StreamWriter sw = new StreamWriter(stream))
                            {
                                sw.Write(xamlShapeString);
                                sw.Flush();
                                stream.Seek(0, SeekOrigin.Begin);
                                Shape shape = XamlReader.Load(stream) as Shape;
                                drawingCanvas.Children.Add(shape);
                            }
                        }
                    }
                }
            }
            else
            {

            }
        }

		// Xử lý new command
        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (drawingCanvas.Children.Count != 0)
            {
                MessageBoxResult result = MessageBox.Show("Close current paint and create new?", "My Paint", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result != MessageBoxResult.OK)
                {
                    return;
                }
            }
            drawingCanvas.Children.Clear();
            RemoveAdorner();
        }

        // Hàm xử lý khi Drag thumb resize canvas bottom right
        void ResizeCanvasRightBottom(object sender, DragDeltaEventArgs e)
        {
            RemoveAdorner();
            double h = drawingCanvas.Height + e.VerticalChange;
            double w = drawingCanvas.Width + e.HorizontalChange;
            if ((w >= 0) && (h >= 0))
            {
                drawingCanvas.Width = w;
                drawingCanvas.Height = h;
                Canvas.SetLeft(canvasResizerRightBottom, Canvas.GetLeft(canvasResizerRightBottom) +
                                        e.HorizontalChange);
                Canvas.SetTop(canvasResizerRightBottom, Canvas.GetTop(canvasResizerRightBottom) +
                                        e.VerticalChange);
            }
        }

		// Đóng ứng dụng
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
