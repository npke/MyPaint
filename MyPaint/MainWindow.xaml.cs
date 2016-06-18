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
using System.Reflection;
using MyPaintPlugin;

namespace MyPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Hình sẽ vẽ
        MyShape myShape;

        // Các đối tượng là tham số đầu vào/thuộc tính khi vẽ một hình
        Point startPoint, endPoint;
        Brush strokeBrush, fillBrush, textBrush;
        double strokeThickness, top, left;
        DoubleCollection dashCollection = new DoubleCollection { };


        // Các biến đánh dấu trạng thái con trở chuột
        bool isMouseDowned = false;
        bool isDragging = false;
        bool isMoved = false;

        // Kiểu enum xác định hình sẽ vẽ và chế độ
        
        enum MODE { DRAW = 1, SELECT = 2 }

        String shapeToDraw = "Line";
        MODE drawMode = MODE.DRAW;

        public static Shape selectedShape = null;
        public static TextBox selectedTextBox = null;
        public static UIElement selectedUIElement = null;

        public static AdornerLayer adornerLayer;


        // Đếm số lần thực hiện paste một đối tượng
        public static int iPaste = 0;
        public static double iLeft = 0, iTop = 0;

        // Đối tượng quản lý các shape toggle button
        private ToggleButtonManager shapeToggleButtonManager = new ToggleButtonManager();

        public MainWindow()
        {
            InitializeComponent();
            InitializeShape();
            CommandManager.PopulateListCmd();
            AddShapeToggleButton();
            ShapeFactory.PopulateBuiltInShape();
            loadPlugin();
            
        }

        private void AddShapeToggleButton()
        {
            shapeToggleButtonManager.AddToggleButton(tglbtnEllipse);
            shapeToggleButtonManager.AddToggleButton(tglbtnLine);
            shapeToggleButtonManager.AddToggleButton(tglbtnRectangle);
            shapeToggleButtonManager.AddToggleButton(tglbtnText);
            shapeToggleButtonManager.AddToggleButton(tglbtnImage);
            shapeToggleButtonManager.AddToggleButton(tglbtnSelect);
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
            if (tglbtnSelect.IsChecked == true)
                drawMode = MODE.SELECT;
            else
                drawMode = MODE.DRAW;

            shapeToggleButtonManager.CheckButton(tglbtnSelect);
        }


        // Đánh dấu đối tượng sẽ vẽ là đường thằng
        private void drawLine(object sender, RoutedEventArgs e)
        {
            drawShape("Line", tglbtnLine);
        }

        // Đánh dấu đối tượng sẽ vẽ là hình chữ nhật
        private void drawRectangle(object sender, RoutedEventArgs e)
        {
            drawShape("Rectangle", tglbtnRectangle);
        }

        // Đánh dấu đối tượng sẽ vẽ là hình elíp
        private void drawEllipse(object sender, RoutedEventArgs e)
        {
            drawShape("Ellipse", tglbtnEllipse);
        }

        // Đánh dấu đối tượng sẽ vẽ là hình mũi tên
        private void drawArrow()
        {
            drawShape("Arrow", null);
        }

        // Đánh dấu đối tượng sẽ vẽ là hình tam giác
        private void drawTriangle()
        {
            drawShape("Triangle", null);
        }

        // Đánh dấu đối tượng sẽ vẽ là hình ngôi sao
        private void drawStar()
        {
            drawShape("Star", null);
        }

        // Đánh dấu đối tượng sẽ vẽ là hình trái tim
        private void drawHeart()
        {
            drawShape("Heart", null);
        }

        private void drawShape(string shapeName, ToggleButton tglBtnShape)
        {
            RemoveAdorner();
            shapeToDraw = shapeName;
            shapeToggleButtonManager.CheckButton(tglBtnShape);
            drawMode = MODE.DRAW;
        }

        // Xử lý khi bấm chuột trên canvas
        private void drawingCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Xử lý cho thumb resize canvas
            if (e.Source == canvasResizerRightBottom)
                return;

            // Lấy vị trí con trỏ chuột bấm xuống và đánh dấu trạng thái chuột là đã bấm
            startPoint = e.GetPosition(drawingCanvas);
            isMouseDowned = true;

            // Nếu double click thì tạo textbox tại vị trí vừa click
            if (tglbtnText.IsChecked == true && e.Source == drawingCanvas)
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
                    prepareToMoveElement();
                    e.Handled = true;
                    return;
                }

                // Nếu có đối tượng vừa vẽ mà lại bấm lên canvas hoặc đối tượng khác
                // => Remove lớp Adorner của đối tượng vừa vẽ chuẩn bị vẽ đối tượng mới
                if (selectedUIElement != null && e.Source != selectedUIElement  && selectedTextBox == null)
                    // Remove Adorner layer
                    RemoveAdorner();

                createNewShape(e);
            }

            // Nếu ở chế độ chọn
            // Nếu click lên một đối tượng thì cho phép move, resize đối tượng đó
            if (drawMode == MODE.SELECT)
                moveResizeElement(e);
            e.Handled = true;
        }

        private void moveResizeElement(MouseButtonEventArgs e)
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

        private void createNewShape(MouseButtonEventArgs e)
        {
            // Dùng lớp ShapeFactory để tạo ra đối tượng hình theo ý muốn.
            Point position = e.GetPosition(drawingCanvas);
            myShape = ShapeFactory.GetShape(shapeToDraw);
            myShape.StartPoint = position;
            myShape.EndPoint = position;
            myShape.Draw(drawingCanvas.Children);
        }

        private void prepareToMoveElement()
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
                if (!updateShapeOnMouseMove(e))
                    return;
            }

            // Nếu ở chế độ chọn => cho phép di chuyển, thay đổi kích thước
            if ((drawMode == MODE.SELECT && selectedUIElement != null) || selectedUIElement != null)
            {
                if (!rotateShape(e))
                    return;

                // Kiểm tra có phải người dùng đang thực hiện thao tác Drag
                if ( (isDragging == false) &&
                    ( (Math.Abs(e.GetPosition(drawingCanvas).X -  startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) || 
                        (Math.Abs(e.GetPosition(drawingCanvas).Y -  startPoint.Y) > SystemParameters.MinimumVerticalDragDistance) ) )
                    isDragging = true;

                // Thực hiện di chuyển đối tượng
                if (isDragging)
                    moveShape(e);
            }
        }

        private void moveShape(MouseEventArgs e)
        {
            Point currentPoint = e.GetPosition(drawingCanvas);

            Canvas.SetTop(selectedUIElement, currentPoint.Y - (startPoint.Y - top));
            Canvas.SetLeft(selectedUIElement, currentPoint.X - (startPoint.X - left));
            isMoved = true;
        }

        private bool rotateShape(MouseEventArgs e)
        {
            // Kiểm tra phím Ctrl có đang được bấm thì sẽ thực hiện xoay đối tượng
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)
            || Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                // Xác định các điểm tâm và điểm tạo góc
                Point centerPoint = new Point();
                endPoint = e.GetPosition(drawingCanvas);

                // Không cho phép xoay đường thẳng
                if (selectedUIElement is Line)
                    return false;

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
                return false;
            }

            return true;
        }

        private bool updateShapeOnMouseMove(MouseEventArgs e)
        {
            // Nhả chuột
            if (e.LeftButton == MouseButtonState.Released)
                return false;

            if (!((Math.Abs(e.GetPosition(drawingCanvas).X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(drawingCanvas).Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                return false;

            // Lấy ví trị chuột mỗi khi di chuyển
            endPoint = e.GetPosition(drawingCanvas);

            // ??? Chuyển thành hàm, chỉnh lại thông số
            // Thiết lập các thuộc tính cho đối tượng cần vẽ
            if (startPoint.Equals(endPoint))
                return false;

            updateShapeProperties();

            // Gọi hàm vẽ đối tượng tương ứng, đây là đối tượng xem trước
            myShape.Draw(drawingCanvas.Children);

            return true;
        }

        private void updateShapeProperties()
        {
            myShape.StrokeBrush = strokeBrush;
            myShape.FillBrush = fillBrush;
            myShape.StrokeThickness = strokeThickness;
            myShape.StartPoint = startPoint;
            myShape.EndPoint = endPoint;
            myShape.DashCollection = dashCollection;
        }

		
		

		// Phương thức sao chép một đối tượng
        // Code tham khảo từ StackOverFlow
        public static UIElement DeepClone(UIElement element)
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
                CommandManager.SnapCanvas(ref drawingCanvas);
            }

        	// Nếu ở chế độ vẽ
            if (drawMode == MODE.DRAW)
            {
                if (!addAdornerWhenMouseUp(e))
                    return;
            }
            
            // Nếu ở chế độ chọn
            if (drawMode == MODE.SELECT || selectedUIElement != null)
            {
            	// Bỏ đánh dấu chuột đang bấm xuống và đang thực hiện Drag
                if (isMouseDowned)
                    isDragging = false;
            }

            // Đưa con trỏ chuột về hình dạng bình thường
            this.Cursor = Cursors.Arrow;
        }

        private bool addAdornerWhenMouseUp(MouseButtonEventArgs e)
        {
            endPoint = e.GetPosition(drawingCanvas);
            if (selectedUIElement == null)
            {
                if (myShape == null)
                    return false;

                try
                {
                    if (myShape.DrawedElement == null)
                        return false;

                    AddAdorner(myShape.DrawedElement);
                    CommandManager.SnapCanvas(ref drawingCanvas);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "1");
                }
            }
            myShape = null;
            return true;
        }

        // Xử lý phím bấm 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
				// Xử lý thao tác xóa
                if (selectedUIElement != null)
                {
                    CommandManager.SnapCanvas(ref drawingCanvas);
                    drawingCanvas.Children.Remove(selectedUIElement);
                }

                selectedUIElement = null;
                selectedShape = null;
                selectedTextBox = null;
            }
			
			// Bấm - để giảm ZIndex của một đối tượng trong canvas
            if (e.Key == Key.Subtract && selectedUIElement != null)
                Canvas.SetZIndex(selectedUIElement, Canvas.GetZIndex(selectedUIElement) - 1);
			
			// Bấm + để tăng ZIndex của một đối tượng trong canvas
            if (e.Key == Key.Add && selectedUIElement != null)
                Canvas.SetZIndex(selectedUIElement, Canvas.GetZIndex(selectedUIElement) + 1);
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
                CommandManager.SnapCanvas(ref drawingCanvas);
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
                CommandManager.SnapCanvas(ref drawingCanvas);
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
                adornerLayer.Add(new ResizingLineAdorner(uiElement));
            else
                adornerLayer.Add(new ResizingAdorner(uiElement));

            selectedUIElement = uiElement;
            if (uiElement is Shape)
                selectedShape = (Shape)uiElement;
            else if (uiElement is TextBox)
                selectedTextBox = (TextBox)uiElement;
        }
		
		// Phương thức thêm văn bản vào canvas
        private void addText(double Top, double Left)
        {
            shapeToggleButtonManager.CheckButton(null);

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
                textBox.FontWeight = FontWeights.Bold;

            if (tglbtnItalic.IsChecked == true)
                textBox.FontStyle = FontStyles.Italic;

            if (tglbtnUnderline.IsChecked == true)
                textBox.TextDecorations = TextDecorations.Underline;

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
            CommandManager.CallItemCmd("copy", ref drawingCanvas, ref canvasResizerRightBottom);

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
            CommandManager.CallItemCmd("cut", ref drawingCanvas, ref canvasResizerRightBottom);

        }

		// Kiểm tra trước khi thực hiện thao tác paste
        private void PasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsText(TextDataFormat.Xaml);
        }

		// Thực hiện paste 
        private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandManager.CallItemCmd("paste", ref drawingCanvas, ref canvasResizerRightBottom);
        }

		// Kiểm tra có thực hiện được thao tác undo hay không
        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CommandManager.UndoStack.Count() > 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

		// Xử lý undo
        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandManager.BackWard(ref drawingCanvas);
        }

		// Kiểm tra có thực hiện được thao tác redo hay không
        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CommandManager.RedoStack.Count() > 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

		// Thực hiện xử lý redo
        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandManager.ForWard(ref drawingCanvas);
        }

		// Phương thức thêm hình ảnh vào canvas 
		// Chưa xử lý xong
        private void addImage(object sender, RoutedEventArgs e)
        {
            RemoveAdorner();
            tglbtnEllipse.IsChecked = false;
            tglbtnLine.IsChecked = false;
            tglbtnText.IsChecked = false;
            tglbtnRectangle.IsChecked = false;
            tglbtnSelect.IsChecked = false;
            if (drawMode == MODE.SELECT)
                drawMode = MODE.DRAW;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "All files |*.*|Bitmap (*.bmp)|*.bmp|PNG (*.png)|*.png|GIF (*.gif)|*.gif|JPEG (*.jpeg)|*.jpg";

            if (openFileDialog.ShowDialog() == false)
                return;

            string fileExtension = System.IO.Path.GetExtension(openFileDialog.FileName);
            fileExtension = fileExtension.ToLower();
            if (fileExtension != ".jpg" && fileExtension != ".bmp" && fileExtension != ".gif" && fileExtension != ".png")
            {
                MessageBox.Show("Please choose an image file", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Image img = new Image();
            var uriSource = new Uri(openFileDialog.FileName);
            img.Source = new BitmapImage(uriSource);
            drawingCanvas.Children.Add(img);
            AddAdorner(img);
            CommandManager.SnapCanvas(ref drawingCanvas);
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
                CommandManager.SnapCanvas(ref drawingCanvas);
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
                CommandManager.SnapCanvas(ref drawingCanvas);
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
                CommandManager.SnapCanvas(ref drawingCanvas);
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
                    CommandManager.SnapCanvas(ref drawingCanvas);
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
                CommandManager.SnapCanvas(ref drawingCanvas);  
            }
        }

		// Thực hiện cập nhật khi thuộc tính Italic thay đổi
        private void tglbtnItalic_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.FontStyle = FontStyles.Italic;
                CommandManager.SnapCanvas(ref drawingCanvas);
            }
        }

		// Thực hiện cập nhật khi thuộc tính Underline thay đổi
        private void tglbtnUnderline_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.TextDecorations = TextDecorations.Underline;
                CommandManager.SnapCanvas(ref drawingCanvas);
            }
        }
		
		// Thực hiện cập nhật khi thuộc tính Bold thay đổi
        private void tglbtnBold_Unchecked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.FontWeight = FontWeights.Normal;
                CommandManager.SnapCanvas(ref drawingCanvas);
            }
        }

		// Thực hiện cập nhật khi thuộc tính Italic thay đổi
        private void tglbtnItalic_Unchecked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.FontStyle = FontStyles.Normal;
                CommandManager.SnapCanvas(ref drawingCanvas);
            }
        }

		// Thực hiện cập nhật khi thuộc tính Underline thay đổi
        private void tglbtnUnderline_Unchecked(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.TextDecorations = null;
                CommandManager.SnapCanvas(ref drawingCanvas);
            }
        }

		// Xử lý khi người dúng bấm nút xóa
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommandManager.CallItemCmd("delete", ref drawingCanvas, ref canvasResizerRightBottom);
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
                CommandManager.SnapCanvas(ref drawingCanvas);
            }
        }
		
		// Xóa bỏ background văn bản
        private void RemoveTextBackground(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.Background = Brushes.Transparent;
                CommandManager.SnapCanvas(ref drawingCanvas);
            }
        }
		
		// Xóa bỏ viền văn bản
        private void RemoveTextBorder(object sender, RoutedEventArgs e)
        {
            if (selectedTextBox != null)
            {
                selectedTextBox.BorderBrush = Brushes.Transparent;
                CommandManager.SnapCanvas(ref drawingCanvas);
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
            drawingCanvas.Children.Remove(canvasResizerRightBottom);
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
                CommandManager.CallItemCmd("save", ref drawingCanvas,ref canvasResizerRightBottom,saveFileDialog.FileName,renderBitmap);
                this.Title = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + " - My Paint";
            }
        }


		// Xử lý open command
        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = " Paint (*.paint)|*.paint|Bitmap (*.bmp)|*.bmp|PNG (*.png)|*.png|GIF (*.gif)|*.gif|JPEG (*.jpeg)|*.jpg";
            this.Title = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName) + " - My Paint";

            CommandManager.CallItemCmd("open", ref drawingCanvas, ref canvasResizerRightBottom);
        }



		// Xử lý new command
        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandManager.CallItemCmd("new", ref drawingCanvas, ref canvasResizerRightBottom);
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

        // Chức năng xoay canvas
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Options op = Options.GetOptionsInstance();
            op.rotateCanvas = rotateCanvas;
            op.Show();
            op.Focus();
        }

        private void rotateCanvas(double angle)
        {
            drawingCanvas.LayoutTransform = new RotateTransform(angle);
        }

		// Đóng ứng dụng
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void triangleShape_Click(object sender, RoutedEventArgs e)
        {
            drawTriangle();
        }

        private void arrowShape_Click(object sender, RoutedEventArgs e)
        {
            drawArrow();
        }

        private void starShape_Click(object sender, RoutedEventArgs e)
        {
            drawStar();
        }

        private void heartShape_Click(object sender, RoutedEventArgs e)
        {
            drawHeart();
        }

        public void loadPlugin()
        {
            // Get list of DLL files in main executable folder
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(exePath) + "\\plugins";
            FileInfo[] fis = new DirectoryInfo(folder).GetFiles("*.dll");

            var plugins = new List<MyShape>();

            // Load all assemblies from current working directory
            foreach (FileInfo fileInfo in fis)
            {
                var domain = AppDomain.CurrentDomain;
                Assembly assembly = domain
                    .Load(AssemblyName.GetAssemblyName(fileInfo.FullName));

                // Get all of the types in the dll
                Type[] types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsClass &&
                        typeof(MyShape).IsAssignableFrom(type))
                    {
                        plugins.Add(Activator.CreateInstance(type) as MyShape);
                    }
                }
            }

            // Invoke all the functions in all the dlls that we found
            foreach (var plugin in plugins)
            {
                MenuItem newItem = new MenuItem();
                newItem.Header = plugin.GetShapeName();
                customShapeMenu.Items.Add(newItem);
                ShapeFactory.AddPrototype(plugin.GetShapeName(), plugin);
            }
        }

        public void customShapeMenuClick(Object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            drawShape(menuItem.Header.ToString(), null);
        }

        private void tglbtnText_Click(object sender, RoutedEventArgs e)
        {
            if (tglbtnText.IsChecked == true)
                shapeToggleButtonManager.CheckButton(tglbtnText);
            else
                shapeToggleButtonManager.CheckButton(null);
        }
    }
}
