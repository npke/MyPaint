using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MyPaint
{
    public class CommandManager
    {
        // Các stack phục vụ mục đích undo và redo
        public static Stack<Command> UndoStack = new Stack<Command>();
        public static Stack<Command> RedoStack = new Stack<Command>();


        // Dictionary để quản lý lệnh open/save/new/delete/cut/copy/paste
        public static Dictionary<string, Command> ListCmd = new Dictionary<string, Command>();

        public static void PopulateListCmd()
        {
            ListCmd.Add("new", new NewCommand(new CanvasState()));
            ListCmd.Add("open", new OpenCommand(new CanvasState()));
            ListCmd.Add("save", new SaveCommand(new CanvasState()));
            ListCmd.Add("copy", new CopyCommand(new CanvasState()));
            ListCmd.Add("cut", new CutCommand(new CanvasState()));
            ListCmd.Add("paste", new PasteCommand(new CanvasState()));
            ListCmd.Add("delete", new DeleteCommand(new CanvasState()));
        }

        // hàm thực hiện một lệnh theo tên
        public static void CallItemCmd(string nameCmd, ref Canvas drawingCanvas, ref Thumb canvasResizerRightBottom)
        {
            try
            {
                ListCmd[nameCmd].Execute(ref drawingCanvas, ref canvasResizerRightBottom);
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: lệnh ngoài hệ thống", "Error", MessageBoxButton.OK);
            }
        }

        public static void AddCurrentSate(List<UIElement> Prams)
        {
            CanvasState doc = new CanvasState(Prams);
            Command cmd = new CanvasCommand(doc);
            UndoStack.Push(cmd);
        }

        public static void BackWard(ref Canvas drawingCanvas)
        {
            Command undoCommand = UndoStack.Pop();
            RedoStack.Push(undoCommand);

            // Nếu trước đó chỉ có 1 trạng thái => Dừng 
            drawingCanvas.Children.Clear();
            if (UndoStack.Count == 0)
            {
                MainWindow.RemoveAdorner();
                return;
            }

            // Lấy trạng thái trước đó và đưa canvas về trạng thái này
            Command RestoreCommand = UndoStack.Pop();
            RestoreCommand.Undo(ref drawingCanvas);

            // Thiết lập selectedUIElement = null
            if (MainWindow.selectedUIElement != null)
            {
                MainWindow.selectedShape = null;
                MainWindow.selectedUIElement = null;
                MainWindow.selectedTextBox = null;
            }

            // Đưa lại trạng thái vào stack
            BackUpCanvasState(RestoreCommand);
        }

        public static void ForWard(ref Canvas drawingCanvas)
        {
            // Lấy trạng thái canvas trong redo stack
            Command redoCommand = RedoStack.Pop();
            UndoStack.Push(redoCommand);

            // Thực hiện khôi phục lại trạng thái đó
            redoCommand.Redo(ref drawingCanvas);
        }

        private static void BackUpCanvasState(Command cmd)
        {
            UndoStack.Push(cmd);
        }


        // Phương thức lưu lại các đối tượng hiện có của Canvas vào trong stack
        public static void SnapCanvas(ref Canvas drawingCanvas)
        {
            List<UIElement> UIEList = new List<UIElement>();
            foreach (UIElement UIE in drawingCanvas.Children)
            {
                UIElement newUIE = MainWindow.DeepClone(UIE);
                UIEList.Add(newUIE);
            }

            AddCurrentSate(UIEList);
        }


    }
}
