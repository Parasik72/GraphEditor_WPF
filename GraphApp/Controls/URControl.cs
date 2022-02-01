using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Controls {
    public class URControl {
        Stack<ICommandUR> UndoStack { get; set; }
        Stack<ICommandUR> RedoStack { get; set; }
        public URControl() {
            UndoStack = new Stack<ICommandUR>();
            RedoStack = new Stack<ICommandUR>();
        }
        public void Undo() {
            if (UndoStack.Count > 0) {
                var command = UndoStack.Pop();
                command.UnExecute();
                RedoStack.Push(command);
            }
        }
        public void Redo() {
            if (RedoStack.Count > 0) {
                var command = RedoStack.Pop();
                command.Execute();
                UndoStack.Push(command);
            }
        }
        public void Execute(ICommandUR command) {
            command.Execute();
            UndoStack.Push(command);
            RedoStack.Clear();
        }
    }
}
