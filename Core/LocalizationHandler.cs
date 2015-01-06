using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Core {
    public class LocalizationHandler : ICommandHandler {
        private readonly Model _model;

        public LocalizationHandler(Model model) {
            _model = model;
        }

        public void Handle(BaseCommand command) {
            Handle((dynamic) command);
        }

        private void Handle(AddLanguageCommand commmand) {
            _model.AddLanguage(commmand.LanguageId,
                               commmand.IsoName,
                               commmand.LCID,
                               commmand.DisplayName);
        }

        private void Handle(SetLanguageCommand command) {
            _model.SetLanguage(command.LanguageId,
                               command.LanguageRegion,
                               command.LCID,
                               command.NewDisplayName);
        }

        private void Handle(SetLanguageAsDefaultCommand command) {
            _model.SetLanguageAsDefault(command.LanguageId);
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

        private void Handle(SetLocalizedTextCommand command) {
            _model.SetLocalizedText(command.AreaId,
                                    command.KeyId,
                                    command.TextId,
                                    command.LanguageId,
                                    command.NewText);
        }

        private void Handle(RemoveLocalizedTextCommand command) {
            _model.RemoveLocalizedText(command.TextId);
        }

        private void Handle(AddLocalizationKeyCommand command) {
            _model.AddLocalizationKey(command.AreaId,
                                      command.KeyId,
                                      command.KeyName);
        }

        private void Handle(SetLocalizationKeyNameCommand command) {
            _model.SetLocalizationKey(command.LocalizationKeyId,
                                          command.AreaId,
                                          command.NewKeyName);
        }

        private void Handle(RemoveLocalizationKeyCommand command) {
            _model.RemoveLocalizationKey(command.LocalizationKeyId);
        }

        private void Handle(AddAreaCommand command) {
            _model.AddArea(command.AreaId,
                           command.Name,
                           command.Comment,
                           command.Image);
        }

        private void Handle(SetAreaCommand command) {
            _model.SetArea(command.AreaId,
                           command.NewName,
                           command.NewComment,
                           command.NewImage);
        }

        private void Handle(RemoveAreaCommand command) {
            _model.RemoveArea(command.AreaId);
        }

        private void Handle(SaveCommand command) {
            _model.Save();
        }

        private void Handle(SaveAsCommand command) {
            _model.SaveAs(command.FilePath);
        }

        private void Handle(NewCommand command) {
            _model.New();
        }

        private void Handle(LoadCommand command) {
            _model.Load(command.FilePath);
        }

        private void Handle(ProduceResourceFileCommand command) {
            _model.ProduceResourceFile(command.ResourceType,
                                       command.FilePath);
        }

        private void Handle(CommandSequence commands) {
            foreach (BaseCommand command in commands.Sequence)
                Handle((dynamic) command);
        }

        public BaseCommand BuildUndoCommand(BaseCommand command) {
            return command.CanUndo() ? BuildUndoCommand((dynamic) command) : null;
        }

        private BaseCommand BuildUndoCommand(AddLanguageCommand command) {
            return new RemoveLanguageCommand(command.LanguageId);
        }

        private BaseCommand BuildUndoCommand(SetLanguageCommand command) {
            var language = _model.RetriveLanguage(command.LanguageId);
            return new SetLanguageCommand(command.LanguageId,
                                          language.LanguageRegion,
                                          language.LCID,
                                          language.DisplayName);
        }

        private BaseCommand BuildUndoCommand(SetLanguageAsDefaultCommand command) {
            var language = _model.RetriveLanguages()
                                 .Single(c => c.IsDefault);

            return new SetLanguageAsDefaultCommand(language.Id);
        }

        private BaseCommand BuildUndoCommand(RemoveLanguageCommand command) {
            var undoCommands = new List<BaseCommand>();
            var language = _model.RetriveLanguage(command.LanguageId);
            var textsThatUsesThisLanguage = _model.RetriveLocalizedTexts()
                                                  .Where(c => c.LanguageId == command.LanguageId)
                                                  .ToList();
            //create command sequence
            undoCommands.Add(new AddLanguageCommand(language.Id,
                                                    language.LanguageRegion,
                                                    language.LCID,
                                                    language.DisplayName));
            if (language.IsDefault)
                undoCommands.Add(new SetLanguageAsDefaultCommand(language.Id));

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

        private BaseCommand BuildUndoCommand(AddLocalizedTextCommand command) {
            return new RemoveLocalizedTextCommand(command.TextId);
        }

        private BaseCommand BuildUndoCommand(SetLocalizedTextCommand command) {
            var key = _model.RetriveLocalizationKey(command.KeyId);
            var text = _model.RetriveLocalizedText(command.TextId);
            return new SetLocalizedTextCommand(text.Id,
                                               text.KeyId,
                                               key.AreaId,
                                               text.LanguageId,
                                               text.Text);
        }

        private BaseCommand BuildUndoCommand(RemoveLocalizedTextCommand command) {
            var text = _model.RetriveLocalizedText(command.TextId);
            var key = _model.RetriveLocalizationKey(text.KeyId);
            return new AddLocalizedTextCommand(key.AreaId,
                                               key.Id,
                                               text.Id,
                                               text.LanguageId,
                                               text.Text);
        }

        private BaseCommand BuildUndoCommand(AddLocalizationKeyCommand command) {
            return new RemoveLocalizationKeyCommand(command.KeyId);
        }

        private BaseCommand BuildUndoCommand(SetLocalizationKeyNameCommand command) {
            var key = _model.RetriveLocalizationKey(command.LocalizationKeyId);
            return new SetLocalizationKeyNameCommand(key.Id,
                                                     command.AreaId,
                                                     key.Key);
        }

        private BaseCommand BuildUndoCommand(RemoveLocalizationKeyCommand command) {
            var sequence = new List<BaseCommand>();

            var key =_model.RetriveLocalizationKey(command.LocalizationKeyId);

            sequence.Add(new AddLocalizationKeyCommand(key.AreaId,
                                                       key.Id,
                                                       key.Key));

            var textRenmovalToUndo = _model.RetriveLocalizedTexts()
                                           .Where(c => c.KeyId == command.LocalizationKeyId)
                                           .ToList();

            sequence.AddRange(textRenmovalToUndo.Select(text => new AddLocalizedTextCommand(key.AreaId,
                                                                                            text.KeyId,
                                                                                            text.Id,
                                                                                            text.LanguageId,
                                                                                            text.Text)));

            return new CommandSequence(sequence);
        }

        private BaseCommand BuildUndoCommand(AddAreaCommand command) {
            return new RemoveAreaCommand(command.AreaId);
        }

        private BaseCommand BuildUndoCommand(SetAreaCommand command) {
            var area =_model.RetriveArea(command.AreaId);
            return new SetAreaCommand(area.Id,
                                      area.Name,
                                      area.Comment,
                                      area.Image);
        }

        private BaseCommand BuildUndoCommand(RemoveAreaCommand command) {
            var sequence = new List<BaseCommand>();

            var area = _model.RetriveArea(command.AreaId);

            sequence.Add(new AddAreaCommand(area.Id,
                                            area.Name,
                                            area.Comment,
                                            area.Image));

            var keys = _model.RetriveLocalizationKeys()
                             .Where(c => c.AreaId == command.AreaId)
                             .ToDictionary(c => c.Id);

            sequence.AddRange(keys.Values.Select(key => new AddLocalizationKeyCommand(key.AreaId,
                                                                                      key.Id,
                                                                                      key.Key)));

            var texts = _model.RetriveLocalizedTexts()
                              .Where(c => keys.ContainsKey(c.KeyId));

            sequence.AddRange(texts.Select(text => new AddLocalizedTextCommand(area.Id,
                                                                               text.KeyId,
                                                                               text.Id,
                                                                               text.LanguageId,
                                                                               text.Text)));

            return new CommandSequence(sequence);
        }

        private BaseCommand BuildUndoCommand(CommandSequence commands) {
            var undoCommands = commands.Sequence.Select(command => BuildUndoCommand((dynamic) command))
                                       .Cast<BaseCommand>()
                                       .ToList();

            return new CommandSequence(undoCommands);
        }
    }

    public class SetLanguageAsDefaultCommand : BaseCommand {
        public Guid LanguageId { get; private set; }

        public SetLanguageAsDefaultCommand(Guid languageId) {
            LanguageId = languageId;
        }
    }

    public interface ICommandHandler {
        void Handle(BaseCommand command);
        BaseCommand BuildUndoCommand(BaseCommand command);
    }

    public abstract class BaseCommand {
        public virtual bool ClearStack() {
            return false;
        }

        public virtual bool CanUndo() {
            return true;
        }
    }

    public class CommandSequence : BaseCommand {
        public IEnumerable<BaseCommand> Sequence { get; private set; }

        public CommandSequence(IEnumerable<BaseCommand> commands) {
            Sequence = commands;
        }
    }

    public class AddLanguageCommand : BaseCommand {
        public Guid LanguageId { get; private set; }
        public string IsoName { get; private set; }
        public string DisplayName { get; private set; }
        public int LCID { get; private set; }

        public AddLanguageCommand(Guid languageId,
                                  string isoName,
                                  int lcid,
                                  string displayName) {
            LanguageId = languageId;
            IsoName = isoName;
            LCID = lcid;
            DisplayName = displayName;
        }
    }

    public class SetLanguageCommand : BaseCommand {
        public Guid LanguageId { get; private set; }
        public string LanguageRegion { get; private set; }
        public int LCID { get; private set; }
        public string NewDisplayName { get; private set; }

        public SetLanguageCommand(Guid languageId,
                                  string languageRegion,
                                  int lcid,
                                  string newDisplayName) {
            LanguageId = languageId;
            LanguageRegion = languageRegion;
            LCID = lcid;
            NewDisplayName = newDisplayName;
        }
    }

    public class RemoveLanguageCommand : BaseCommand {
        public Guid LanguageId { get; private set; }

        public RemoveLanguageCommand(Guid languageId) {
            LanguageId = languageId;
        }
    }

    public class AddLocalizedTextCommand : BaseCommand {
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

    public class SetLocalizedTextCommand : BaseCommand {
        public Guid TextId { get; private set; }
        public Guid KeyId { get; private set; }
        public Guid AreaId { get; private set; }
        public Guid LanguageId { get; private set; }
        public string NewText { get; private set; }

        public SetLocalizedTextCommand(Guid textId,
                                       Guid keyId,
                                       Guid areaId,
                                       Guid languageId,
                                       string newText) {
            TextId = textId;
            KeyId = keyId;
            AreaId = areaId;
            LanguageId = languageId;
            NewText = newText;
        }
    }

    public class RemoveLocalizedTextCommand : BaseCommand {
        public Guid TextId { get; private set; }

        public RemoveLocalizedTextCommand(Guid textId) {
            TextId = textId;
        }
    }

    public class AddLocalizationKeyCommand : BaseCommand {
        public Guid AreaId { get; private set; }
        public Guid KeyId { get; private set; }
        public string KeyName { get; private set; }

        public AddLocalizationKeyCommand(Guid areaId,
                                         Guid keyId,
                                         string keyName) {
            AreaId = areaId;
            KeyId = keyId;
            KeyName = keyName;
        }
    }

    public class SetLocalizationKeyNameCommand : BaseCommand {
        public Guid LocalizationKeyId { get; private set; }
        public Guid AreaId { get; private set; }
        public string NewKeyName { get; private set; }

        public SetLocalizationKeyNameCommand(Guid localizationKeyId, Guid areaId, string newKeyName) {
            LocalizationKeyId = localizationKeyId;
            AreaId = areaId;
            NewKeyName = newKeyName;
        }
    }

    public class RemoveLocalizationKeyCommand : BaseCommand {
        public Guid LocalizationKeyId { get; private set; }

        public RemoveLocalizationKeyCommand(Guid localizationKeyId) {
            LocalizationKeyId = localizationKeyId;
        }
    }

    public class AddAreaCommand : BaseCommand {
        public Guid AreaId { get; private set; }
        public string Name { get; private set; }
        public string Comment { get; set; }
        public byte[] Image { get; set; }

        public AddAreaCommand(Guid areaId,
                              string name,
                              string comment,
                              byte[] image) {
            AreaId = areaId;
            Name = name;
            Comment = comment;
            Image = image;
        }
    }

    public class SetAreaCommand : BaseCommand {
        public Guid AreaId { get; private set; }
        public string NewName { get; private set; }
        public string NewComment { get; private set; }
        public byte[] NewImage { get; private set; }

        public SetAreaCommand(Guid areaId,
                              string newName,
                              string newComment,
                              byte[] newImage) {
            AreaId = areaId;
            NewName = newName;
            NewComment = newComment;
            NewImage = newImage;
        }
    }

    public class RemoveAreaCommand : BaseCommand {
        public Guid AreaId { get; private set; }

        public RemoveAreaCommand(Guid areaId) {
            AreaId = areaId;
        }
    }

    public class SaveCommand : BaseCommand {
        public override bool CanUndo() {
            return false;
        }
    }
    
    public class NewCommand : BaseCommand {
        public override bool ClearStack() {
            return true;
        }

        public override bool CanUndo() {
            return false;
        }
    }

    public class SaveAsCommand : BaseCommand {
        public string FilePath { get; private set; }

        public SaveAsCommand(string filePath) {
            FilePath = filePath;
        }

        public override bool CanUndo() {
            return false;
        }
    }

    public class LoadCommand : BaseCommand {
        public string FilePath { get; private set; }

        public LoadCommand(string filePath) {
            FilePath = filePath;
        }

        public override bool ClearStack() {
            return true;
        }

        public override bool CanUndo() {
            return false;
        }
    }

    public class ProduceResourceFileCommand : BaseCommand {
        public string FilePath { get; private set; }
        public ResourceType ResourceType { get; private set; }

        public ProduceResourceFileCommand(ResourceType resourceType,
                                          string filePath) {
            FilePath = filePath;
            ResourceType = resourceType;
        }

        public override bool CanUndo() {
            return false;
        }
    }
}