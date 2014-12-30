using Core;
using Moq;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class CommandInvokerTests {
        private Mock<ICommandHandler> _handler;

        [SetUp]
        private void Setup() {
            _handler = new Mock<ICommandHandler>();
        }

        [Test]
        public void Invoke_SimpleCommand_CreatesUndoCommandAndIsSentToHandler() {
            var command = new TestCommand();
            var undoCommand = new TestCommand();

            _handler.Setup(c => c.BuildUndoCommand(command))
                    .Returns(undoCommand);

            var invoker = CreateaDefaultCommandHandler();

            invoker.Invoke(command);

            _handler.Verify(c=>c.BuildUndoCommand(command));
            _handler.Verify(c => c.Handle(command));
        }

        private CommandInvoker CreateaDefaultCommandHandler() {
            return new CommandInvoker(_handler.Object);
        }
    }

    public class TestCommand : BaseCommand { }
}
