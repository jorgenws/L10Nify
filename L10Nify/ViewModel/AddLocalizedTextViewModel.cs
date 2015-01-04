using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Core;

namespace L10Nify {
    public class AddLocalizedTextViewModel : Screen {
        public IEnumerable<Area> Areas {
            get {
                return _queryModel.RetriveAreas()
                                  .ToList();
            }
        }

        public IEnumerable<LocalizationKey> Keys {
            get {
                return _queryModel.RetriveLocalizationKeys()
                                  .Where(c => c.AreaId == AreaId)
                                  .ToList();
            }
        }

        public IEnumerable<Language> Languages {
            get {
                return _queryModel.RetriveLanguages()
                                  .ToList();
            }
        }

        public Guid LanguageId { get; set; }

        private Guid _areaId;
        public Guid AreaId {
            get { return _areaId; }
            set {
                _areaId = value;
                NotifyOfPropertyChange(() => Keys);
                NotifyOfPropertyChange(() => SelectedArea);
            }
        }

        public AreaViewModel SelectedArea {
            get { return _areaViewModelFactory.Create(_queryModel.RetriveArea(AreaId)); }
        }

        public Guid KeyId { get; set; }
        public string Text { get; set; }

        private readonly IQueryModel _queryModel;
        private readonly IAreaViewModelFactory _areaViewModelFactory;
        
        public AddLocalizedTextViewModel(IQueryModel queryModel, IAreaViewModelFactory areaViewModelFactory) {
            _queryModel = queryModel;
            _areaViewModelFactory = areaViewModelFactory;
        }

        public void Ok() {
            TryClose(true);
        }

        public void Cancel() {
            TryClose(false);
        }
    }
}