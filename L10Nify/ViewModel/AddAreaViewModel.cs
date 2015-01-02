using System.IO;
using System.Windows.Forms;
using Screen = Caliburn.Micro.Screen;

namespace L10Nify {
    public class AddAreaViewModel : Screen {
        public string AreaName { get; set; }
        public string Comment { get; set; }
        public byte[] Image { get; set; }

        public void AddImage() {
            //open file dialog

            var ofd = new OpenFileDialog();
            ofd.Filter = "Image files|*.png;*.jpg;*.jpeg;*.gif;";
            ofd.FilterIndex = 0;

            if (ofd.ShowDialog() == DialogResult.OK &&
                ofd.CheckPathExists) {
                using (var fs = new FileStream(ofd.FileName, FileMode.Open)) {
                    using (var ms = new MemoryStream()) {
                        fs.CopyTo(ms);
                        ms.Position = 0;
                        Image = ms.ToArray();
                    }
                }
            }

        }

        public void Ok() {
            TryClose(true);
        }

        public void Cancel() {
            TryClose(false);
        }
    }
}
