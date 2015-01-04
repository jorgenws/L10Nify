using System;
using System.IO;
using System.Windows.Media.Imaging;
using Core;

namespace L10Nify {
    public class AreaViewModel {
        public Guid Id {
            get { return _area.Id; }
        }

        public string Name {
            get { return _area.Name; }
        }

        public string Comment {
            get { return _area.Comment; }
        }

        public BitmapImage Image {
            get {
                if (_image == null &&
                    _area != null &&
                    _area.Image != null) {
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
}