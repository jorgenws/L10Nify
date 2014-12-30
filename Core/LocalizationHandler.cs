using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class LocalizationHandler : ICommandHandler {
        private readonly Model _model;

        public LocalizationHandler(Model model) {
            _model = model;
        }

        public void Handle(ICommand command) {
            Handle((dynamic) command);
        }

        private void Handle(AddLanguageCommand commmand) {
            _model.AddLanguage(commmand.LanguageId,
                               commmand.IsoName,
                               commmand.DisplayName);
        }

        private void Handle(RemoveLanguageCommand command) {
            _model.RemoveLanguage(command.LanguageId);
        }

        private void Handle(AddLocalizedTextCommand command) {
            _model.AddLocalizedText(command.AreaId,
                                    command.KeyId,
                                    command.TextId,
                                    command.LanguageId,
                                    command.Text);
        }

        public void Handle(RemoveLocalizedTextCommand command) {
            _model.RemoveLocalizedText(command.TextId);
        }

        private void Handle(CommandSequence commands) {
            foreach (ICommand command in commands.Sequence)
                Handle((dynamic) command);
        }

        public ICommand BuildUndoCommand(ICommand command) {
            return BuildUndoCommand((dynamic) command);
        }

        private ICommand BuildUndoCommand(AddLanguageCommand command) {
            return new RemoveLanguageCommand(command.LanguageId);
        }

        private ICommand BuildUndoCommand(RemoveLanguageCommand command) {
            var undoCommands = new List<ICommand>();
            var language = _model.RetriveLanguage(command.LanguageId);
            var textsThatUsesThisLanguage = _model.RetriveLocalizedTexts()
                                                  .Where(c => c.LanguageId == command.LanguageId)
                                                  .ToList();
            //create command sequence
            undoCommands.Add(new AddLanguageCommand(language.Id,
                                                    language.IsoName,
                                                    language.DisplayName));
            foreach (var text in textsThatUsesThisLanguage) {
                var key = _model.RetriveLocalizationKey(text.KeyId);
                undoCommands.Add(new AddLocalizedTextCommand(key.AreaId,
                                                             text.KeyId,
                                                             text.Id,
                                                             text.LanguageId,
                                                             text.Text));
            }

            return new CommandSequence(undoCommands);
        }

        private ICommand BuildUndoCommand(AddLocalizedTextCommand command) {
            return new RemoveLocalizedTextCommand(command.TextId);
        }

        private ICommand BuildUndoCommand(RemoveLocalizedTextCommand command) {
            var text = _model.RetriveLocalizedText(command.TextId);
            var key = _model.RetriveLocalizationKey(text.KeyId);
            return new AddLocalizedTextCommand(key.AreaId,
                                               key.Id,
                                               text.Id,
                                               text.LanguageId,
                                               text.Text);
        }

        private ICommand BuildUndoCommand(CommandSequence commands) {
            var undoCommands = new List<ICommand>();

            foreach (ICommand command in commands.Sequence.Reverse())
                undoCommands.AddRange(BuildUndoCommand((dynamic) command));

            return new CommandSequence(undoCommands);
        }
    }

    public interface ICommandHandler {
        void Handle(ICommand command);
        ICommand BuildUndoCommand(ICommand command);
    }

    public interface ICommand {}

    public class CommandSequence : ICommand {
        public IEnumerable<ICommand> Sequence { get; private set; }

        public CommandSequence(IEnumerable<ICommand> commands) {
            Sequence = commands;
        }
    }

    public class AddLanguageCommand : ICommand {
        public Guid LanguageId { get; private set; }
        public string IsoName { get; private set; }
        public string DisplayName { get; private set; }

        public AddLanguageCommand(Guid languageId,
                                  string isoName,
                                  string displayName) {
            LanguageId = languageId;
            IsoName = isoName;
            DisplayName = displayName;
        }
    }

    public class RemoveLanguageCommand : ICommand {
        public Guid LanguageId { get; private set; }

        public RemoveLanguageCommand(Guid languageId) {
            LanguageId = languageId;
        }
    }

    public class AddLocalizedTextCommand : ICommand {
        public Guid AreaId { get; private set; }
        public Guid KeyId { get; private set; }
        public Guid TextId { get; private set; }
        public Guid LanguageId { get; private set; }
        public string Text { get; private set; }

        public AddLocalizedTextCommand(Guid areaId,
                                       Guid keyId,
                                       Guid textId,
                                       Guid languageId,
                                       string text) {
            AreaId = areaId;
            KeyId = keyId;
            TextId = textId;
            LanguageId = languageId;
            Text = text;
        }
    }

    public class RemoveLocalizedTextCommand : ICommand {
        public Guid TextId { get; private set; }

        public RemoveLocalizedTextCommand(Guid textId) {
            TextId = textId;
        }
    }
}