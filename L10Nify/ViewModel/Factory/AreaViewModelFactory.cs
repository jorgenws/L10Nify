using Core;

namespace L10Nify {
    public class AreaViewModelFactory : IAreaViewModelFactory {
        public AreaViewModel Create(Area area) {
            return new AreaViewModel(area);
        }
    }

    public interface IAreaViewModelFactory {
        AreaViewModel Create(Area area);
    }
}