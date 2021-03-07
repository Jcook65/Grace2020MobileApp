using Grace2020.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.ViewModels.Instances
{
    public class ImageViewerVM : VMBase
    {
        private string _image;
        public string Image
        {
            get { return _image; }
            set { Set(() => Image, ref _image, value); }
        }
        public ImageViewerVM(string image)
        {
            _image = image;
        }
    }
}
