using Grace2020.ViewModels.Instances;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Instances
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsDetailsVW : ContentPage
    {
        public NewsDetailsVW()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(BindingContext is NewsDetailVM bindingContext)
            {
                var items = bindingContext.News.Images;
                pictureCarousel.ItemsSource = items;
            }
        }
    }
}