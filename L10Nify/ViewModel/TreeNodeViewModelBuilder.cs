using System;
using System.Collections.Generic;
using Core;

namespace L10Nify.ViewModel {
    public class TreeNodeViewModelBuilder : ITreeNodeViewModelBuilder {
        public IEnumerable<ITreeNodeViewModel> Build(IQueryModel queryModel) {
            //This will be some work...
            throw new NotImplementedException();
        }
    }

    public interface ITreeNodeViewModelBuilder {
        IEnumerable<ITreeNodeViewModel> Build(IQueryModel queryModel);
    }
}
