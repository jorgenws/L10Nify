using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace L10Nify {
    public class AreaTreeNodeViewModel : ITreeNodeViewModel {
        public IEnumerable<ITreeNodeViewModel> Children {
            get { return _localizationKeyTreeNodeViewModels; }
        }

        public Guid Id {get { return _areaViewModel.Id; }}
        public string Name { get { return _areaViewModel.Name; }}
        public string Comment { get { return _areaViewModel.Comment; } }
        public BitmapImage Image { get { return _areaViewModel.Image; } }

        private readonly AreaViewModel _areaViewModel;
        private readonly IEnumerable<LocalizationKeyTreeNodeViewModel> _localizationKeyTreeNodeViewModels;

        public AreaTreeNodeViewModel(AreaViewModel areaViewModel,
                                     IEnumerable<LocalizationKeyTreeNodeViewModel> localizationKeyTreeNodeViewModels) {
            _areaViewModel = areaViewModel;
            _localizationKeyTreeNodeViewModels = localizationKeyTreeNodeViewModels;
        }
    }
}