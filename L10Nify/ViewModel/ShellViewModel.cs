using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Core;

namespace L10Nify {
    public class ShellViewModel : PropertyChangedBase,
                                  IShell {
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public IEnumerable<string> Languages {
            get {
                return _queryModel.RetriveLanguages()
                                  .Select(c => c.DisplayName)
                                  .ToList();
            }
        }

        public IEnumerable<AreaViewModel> Areas {
            get {
                return _queryModel.RetriveAreas()
                                  .Select(c => _areaViewModelFactory.Create(c))
                                  .ToList();
            }
        }

        public IEnumerable<string> Keys {
            get {
                return _queryModel.RetriveLocalizationKeys()
                                  .Select(c => c.Key)
                                  .ToList();
            }
        }

        public IEnumerable<string> Texts {
            get {
                return _queryModel.RetriveLocalizedTexts()
                                  .Select(c => c.Text)
                                  .ToList();
            }
        } 


        private readonly IQueryModel _queryModel;
        private readonly ICommandInvoker _commandInvoker;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IAreaViewModelFactory _areaViewModelFactory;
        private readonly IWindowManager _windowManager;

        public ShellViewModel(IQueryModel queryModel,
                              ICommandInvoker commandInvoker,
                              IGuidGenerator guidGenerator,
                              IAreaViewModelFactory areaViewModelFactory,
                              IWindowManager windowManager) {
            _queryModel = queryModel;
            _commandInvoker = commandInvoker;
            _guidGenerator = guidGenerator;
            _areaViewModelFactory = areaViewModelFactory;
            _windowManager = windowManager;

            UndoCommand = new RelayCommand(Undo);
            RedoCommand = new RelayCommand(Redo);
        }

        public void AddLanguage() {
            var vm = new AddLanguageViewModel();
            OpenDataView(vm,
                         c => new AddLanguageCommand(_guidGenerator.Next(),
                                                     c.IsoName,
                                                     c.LanguageDisplayName));
        }

        public void AddArea() {
            var vm = new AddAreaViewModel();
            OpenDataView(vm,
                         c => new AddAreaCommand(_guidGenerator.Next(),
                                                 c.AreaName,
                                                 c.Comment,
                                                 c.Image));
        }

        public void AddLocalizationKey() {
            var vm = new AddLocalizationKeyViewModel(_queryModel);
            OpenDataView(vm,
                         c => new AddLocalizationKeyCommand(c.AreaId,
                                                            _guidGenerator.Next(),
                                                            c.KeyName));
        }

        public void AddLocalizedText() {
            var vm = new AddLocalizedTextViewModel(_queryModel);
            OpenDataView(vm,
                         c => new AddLocalizedTextCommand(c.AreaId,
                                                          c.KeyId,
                                                          _guidGenerator.Next(),
                                                          c.LanguageId,
                                                          c.Text));
        }

        private void OpenDataView<T>(T vm, Func<T, BaseCommand> createCommand) where T : Screen {
            var result = _windowManager.ShowDialog(vm);
            if (result.HasValue && result.Value) {
                _commandInvoker.Invoke(createCommand(vm));
                RefreshView();
            }
        }

        public void Undo() {
            _commandInvoker.Undo();
            RefreshView();
        }

        public void Redo() {
            _commandInvoker.Do();
            RefreshView();
        }

        private void RefreshView() {
            NotifyOfPropertyChange(() => Languages);
            NotifyOfPropertyChange(() => Areas);
            NotifyOfPropertyChange(() => Keys);
            NotifyOfPropertyChange(() => Texts);
        }
    }

    public class AreaViewModel {
        public Guid Id { get { return _area.Id; } }
        public string Name { get { return _area.Name; } }
        public string Comment { get { return _area.Comment; } }

        public BitmapImage Image {
            get {
                if (_image == null &&
                    _area.Image != null) {
                    //var converter = new ImageConverter();
                    //_image = (Image) converter.ConvertFrom(_area.Image);

                    _image = new BitmapImage();
                    _image.BeginInit();
                    _image.StreamSource = new MemoryStream(_area.Image);
                    _image.EndInit();
                    _image.Freeze();
                }

                return _image;
            }
        }

        private BitmapImage _image;

        private readonly Area _area;

        public AreaViewModel(Area area) {
            _area = area;
        }
    }

    public class AreaViewModelFactory : IAreaViewModelFactory {
        public AreaViewModel Create(Area area) {
            return new AreaViewModel(area);
        }
    }

    public interface IAreaViewModelFactory {
        AreaViewModel Create(Area area);
    }
}