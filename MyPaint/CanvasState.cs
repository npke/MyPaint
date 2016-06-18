using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace MyPaint
{
    public class CanvasState
    {
        private List<UIElement> _ListUIE = new List<UIElement>();

        public CanvasState(List<UIElement> ListUIE)
        {
            _ListUIE = ListUIE;
        }

        public CanvasState()
        { 
        
        }

        // Thực hiện khôi phục lại trạng thái đó
        public void RestoreCanvas(ref Canvas drawingCanvas )
        {
            drawingCanvas.Children.Clear();
            foreach (UIElement UIE in _ListUIE)
            {
                drawingCanvas.Children.Add(UIE);
            }
        }

        // Hàm thực hiện copy Shape được chọn.
        public void CopyShape(UIElement selectedUIElement)
        {
            string xaml = XamlWriter.Save(selectedUIElement);
            Clipboard.SetText(xaml, TextDataFormat.Xaml);

            MainWindow.iPaste = 1; // Số lần paste dùng để xác định vị trí cho các đối tượng khi paste
            MainWindow.iLeft = Canvas.GetLeft(selectedUIElement);
            MainWindow.iTop = Canvas.GetTop(selectedUIElement);
        }

        // Hàm thực hiện cắt shape đã được chọn
        public void CutShape(UIElement selectedUIElement,ref Canvas drawingCanvas)
        {
            string xaml = XamlWriter.Save(selectedUIElement);
            Clipboard.SetText(xaml, TextDataFormat.Xaml);
            drawingCanvas.Children.Remove(selectedUIElement);

            MainWindow.iPaste = 1;
            MainWindow.iLeft = Canvas.GetLeft(selectedUIElement);
            MainWindow.iTop = Canvas.GetTop(selectedUIElement);

            MainWindow.selectedShape = null;
            MainWindow.selectedUIElement = null;
            MainWindow.selectedTextBox = null;
        }


        // Hàm thực hiện paste hình
        public void PasteShape(ref Canvas drawingCanvas)
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
                            line.X1 += (10 + line.StrokeThickness) * MainWindow.iPaste;
                            line.X2 += (10 + line.StrokeThickness) * MainWindow.iPaste;
                        }
                        else
                        {
                            Canvas.SetLeft(shape, MainWindow.iLeft + (10 + shape.StrokeThickness) * MainWindow.iPaste);
                            Canvas.SetTop(shape, MainWindow.iTop + (10 + shape.StrokeThickness) * MainWindow.iPaste);
                        }

                        drawingCanvas.Children.Add(shape);
                        CommandManager.SnapCanvas(ref drawingCanvas);
                        MainWindow.RemoveAdorner();
                        MainWindow.AddAdorner(shape);
                        MainWindow.iPaste++;
                    }
                }
            }
        }


        // thực hiện xóa Shape
        public void DeteteShape(ref Canvas drawingCanvas)
        {
            if (MainWindow.selectedUIElement != null)
            {
                CommandManager.SnapCanvas(ref drawingCanvas);
                drawingCanvas.Children.Remove(MainWindow.selectedUIElement);
            }

            MainWindow.selectedUIElement = null;
            MainWindow.selectedShape = null;
        }


        // Thực hiện save
        public void Save(ref Canvas drawingCanvas, ref Thumb canvasResizerRightBottom, string fileName, RenderTargetBitmap renderBitmap)
        {
            SaveFileManager saveFile = new SaveFileManager(drawingCanvas, fileName, renderBitmap);
            saveFile.Save();
        }



        // Xử lý open
        public void Open(ref Canvas drawingCanvas)
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
            //Window.Title = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName) + " - My Paint";
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
                                UIElement shape = XamlReader.Load(stream) as UIElement;
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

        // thực hiện tạo Shape
        public void New(ref Canvas drawingCanvas)
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
            MainWindow.RemoveAdorner();
        }
    }
}
