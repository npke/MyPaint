using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyPaint
{
    public class CommandManager
    {
        // Các stack phục vụ mục đích undo và redo
        public static Stack<Command> UndoStack = new Stack<Command>();
        public static Stack<Command> RedoStack = new Stack<Command>();


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



    }
}
