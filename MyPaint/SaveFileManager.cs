using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace MyPaint
{
    class SaveFileManager
    {
        private System.IO.MemoryStream ms = new System.IO.MemoryStream();
        private BitmapEncoder bitmapEncoder;
        private string fileName;
        private Canvas drawingCanvas;
        private RenderTargetBitmap renderBitmap;


        public SaveFileManager(Canvas drawingCanvas, string fileName, RenderTargetBitmap renderBitmap)
        {
            this.drawingCanvas = drawingCanvas;
            this.fileName = fileName;
            this.renderBitmap = renderBitmap;
        }

        private BitmapEncoder GetBitmapEncoder(string fileExtension)
        {
            switch (System.IO.Path.GetExtension(fileExtension))
            {
                case ".bmp":
                    return new BmpBitmapEncoder();

                case ".png":
                    return new PngBitmapEncoder();

                case ".gif":
                    return new GifBitmapEncoder();

                case ".jpg":
                    return new JpegBitmapEncoder();

                default:
                    return null;
            }
        }

        private void SaveAsBitmapFile()
        {
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            bitmapEncoder.Save(ms);
            ms.Close();
            System.IO.File.WriteAllBytes(fileName, ms.ToArray());
        }

        private void SaveAsMyPaintFile()
        {
            string[] xamlShape = new string[drawingCanvas.Children.Count];
            int i = 0;
            foreach (UIElement mySaveShape in drawingCanvas.Children)
            {
                xamlShape[i] = XamlWriter.Save(mySaveShape);
                i++;
            }
            System.IO.File.WriteAllLines(fileName, xamlShape);
            ms.Close();
        }

        public void Save()
        {
            bitmapEncoder = GetBitmapEncoder(System.IO.Path.GetExtension(fileName));

            if (bitmapEncoder != null)
                SaveAsBitmapFile();
            else
                SaveAsMyPaintFile();
        }
    }
}
