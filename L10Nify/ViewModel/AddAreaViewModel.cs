using Caliburn.Micro;

namespace L10Nify {
    public class AddAreaViewModel : Screen {
        public string AreaName { get; set; }

        public void Ok() {
            TryClose(true);
        }

        public void Cancel() {
            TryClose(false);
        }
    }
}
