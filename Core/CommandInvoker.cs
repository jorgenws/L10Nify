using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class CommandInvoker : ICommandInvoker {
        private readonly Stack<ICommand> _undoStack;
        private readonly Stack<ICommand> _redoStack;

        public CommandInvoker() {
            _undoStack = new Stack<ICommand>();
            _redoStack = new Stack<ICommand>();
        }

        public void Undo() {
            if (!_undoStack.Any()) return;

            var command = _undoStack.Pop();

            //Create redo command(s)
        }

        public void Redo() {
            if(!_redoStack.Any()) return;

            var command = _redoStack.Pop();

            //Create undo command(s)
        }

        public void Invoke(ICommand command) {
            //Create undo

            //
        }
    }

    public interface ICommandInvoker {
        void Undo();
        void Redo();
        void Invoke(ICommand command);
    }
}
