using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class CommandInvoker : ICommandInvoker {
        private readonly ICommandHandler _commandHandler;
        private readonly Stack<DoUndoTuple> _undoStack;
        private readonly Stack<DoUndoTuple> _doStack;

        public CommandInvoker(ICommandHandler commandHandler) {
            _commandHandler = commandHandler;
            _undoStack = new Stack<DoUndoTuple>();
            _doStack = new Stack<DoUndoTuple>();
        }

        public void Do() {
            if (!_doStack.Any())
                return;

            try {
                var tuple = _doStack.Pop();
                _commandHandler.Handle(tuple.Do);
                
                //command will have to clear the stack because the 
                //context has changed (for example a new file or a loaded one).
                if (tuple.Do.ClearStack()) {
                    _undoStack.Clear();
                    _doStack.Clear();                    
                }
                
                //Some operations can not be undone (for example saving).
                //Do not push it to undo stack
                if (tuple.Undo != null)
                    _undoStack.Push(tuple);

            }
            catch (Exception e) {
                throw;
            }
        }

        public void Undo() {
            if (!_undoStack.Any()) 
                return;

            try {
                var tuple = _undoStack.Pop();
                _commandHandler.Handle(tuple.Undo);
                _doStack.Push(tuple);
            }
            catch (Exception e) {
                throw;
            }
        }

        public void Invoke(BaseCommand command) {
            var undoCommand = _commandHandler.BuildUndoCommand(command);
            _doStack.Push(new DoUndoTuple(command,
                                          undoCommand));
            Do();
        }
    }

    public interface ICommandInvoker {
        void Invoke(BaseCommand command);
        void Undo();
        void Do();
    }

    public class DoUndoTuple {
        public BaseCommand Do { get; set; }
        public BaseCommand Undo { get; set; }

        public DoUndoTuple(BaseCommand @do,
                           BaseCommand undo) {
            Do = @do;
            Undo = undo;
        }
    }
}