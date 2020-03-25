using Grace2020.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Collections
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayPickerVW : ContentPage
    {
        public DayPickerVW()
        {
            InitializeComponent();
            //DayCarousel.PeekAreaInsets = Device.RuntimePlatform == "Android" ? new Thickness(0, 310) : new Thickness(0, 200);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            BindingContext = null;
        }
    }
}