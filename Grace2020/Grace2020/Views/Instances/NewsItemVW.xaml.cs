using FFImageLoading.Forms;
using Grace2020.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Instances
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsItemVW : ContentView
    {
        public NewsItemVW()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            newsImage.Source = null;
            if(BindingContext is News item)
            {
                newsImage.Source = item.ImageSourceURL;
            }
        }
    }
}