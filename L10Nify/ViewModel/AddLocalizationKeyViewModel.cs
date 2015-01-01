using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Core;

namespace L10Nify {
    public class AddLocalizationKeyViewModel : Screen {
        public Guid AreaId { get; set; }
        public string KeyName { get; set; }

        public IEnumerable<Area> Areas {
            get { return _queryModel.RetriveAreas(); }
        }

        private readonly IQueryModel _queryModel;

        public AddLocalizationKeyViewModel(IQueryModel queryModel) {
            _queryModel = queryModel;
        }

        public void Ok() {
            TryClose(true);
        }

        public void Cancel() {
            TryClose(false);
        }
    }
}
