using System.Windows;

namespace L10Nify {
    public partial class ShellView : Window {
        public ShellView() {
            InitializeComponent();
        }

        private void QuitClicked(object sender,
                                 RoutedEventArgs e) {
            Close();
        }
    }
}
