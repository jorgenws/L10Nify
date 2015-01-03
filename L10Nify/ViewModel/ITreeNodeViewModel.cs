using System.Collections.Generic;

namespace L10Nify {
    public interface ITreeNodeViewModel {
        IEnumerable<ITreeNodeViewModel> Children { get; }
    }
}