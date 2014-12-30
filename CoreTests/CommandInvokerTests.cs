using Core;
using Moq;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class CommandInvokerTests {
        private Mock<ICommandHandler> _handler;

        [SetUp]
        public void Setup() {
            _handler = new Mock<ICommandHandler>();
        }

        [Test]
        public void Invoke_SimpleCommand_CreatesUndoCommandAndIsSentToHandler() {
            var command = CreateDefaultCommand();
            var undoCommand = CreateDefaultCommand();

            _handler.Setup(c => c.BuildUndoCommand(command))
                    .Returns(undoCommand);

            var invoker = CreateaDefaultCommandInvoker();

            invoker.Invoke(command);

            _handler.Verify(c=>c.BuildUndoCommand(command));
            _handler.Verify(c => c.Handle(command));
        }

        [Test]
        public void Undo_SimpleCommand_RunsTheUndoCommand() {
            var command = CreateDefaultCommand();
            var undoCommand = CreateDefaultCommand();

            _handler.Setup(c => c.BuildUndoCommand(command))
                    .Returns(undoCommand);

            var invoker = CreateaDefaultCommandInvoker();

            invoker.Invoke(command);
            invoker.Undo();

            _handler.Verify(c=>c.BuildUndoCommand(command));
            _handler.Verify(c=>c.Handle(command));
            _handler.Verify(c => c.Handle(undoCommand));
        }

        [Test]
        public void Do_SimpleCommand_RunsTheCommandAgainCommand() {
            var command = CreateDefaultCommand();
            var undoCommand = CreateDefaultCommand();

            _handler.Setup(c => c.BuildUndoCommand(command))
                    .Returns(undoCommand);

            var invoker = CreateaDefaultCommandInvoker();

            invoker.Invoke(command);
            invoker.Undo();
            invoker.Do();

            _handler.Verify(c => c.BuildUndoCommand(command));
            _handler.Verify(c => c.Handle(command), Times.Exactly(2));
            _handler.Verify(c => c.Handle(undoCommand));
        }

        [Test]
        public void Do_SimpleCommandThatClearsTheStack_DoesNotRunTheCommandAgain() {
            var command = CreateDefaultCommand();
            var undoCommand = CreateDefaultCommand();
            var commandThatClearsTheStack = CreateDefaultCommandThatClearsTheStack();

            _handler.Setup(c => c.BuildUndoCommand(command))
                    .Returns(undoCommand);

            var invoker = CreateaDefaultCommandInvoker();

            invoker.Invoke(command);
            invoker.Invoke(commandThatClearsTheStack);
            invoker.Undo();


            //The undo command will not invoke Handle since the second command clears the stack
            _handler.Verify(c => c.Handle(command),
                            Times.Exactly(1));
            _handler.Verify(c => c.Handle(commandThatClearsTheStack));
            _handler.Verify(c => c.Handle(undoCommand),
                            Times.Never());            
        }

        private CommandInvoker CreateaDefaultCommandInvoker() {
            return new CommandInvoker(_handler.Object);
        }

        private TestCommand CreateDefaultCommand() {
            return new TestCommand();
        }

        private TestCommandThatClearsTheStack CreateDefaultCommandThatClearsTheStack() {
            return new TestCommandThatClearsTheStack();
        }
    }

    public class TestCommand : BaseCommand { }

    public class TestCommandThatClearsTheStack : BaseCommand {
        public override bool ClearStack() {
            return true;
        }
    }
}
