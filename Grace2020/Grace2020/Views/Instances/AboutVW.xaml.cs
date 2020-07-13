using Grace2020.Resources;
using Grace2020.Styles;
using Grace2020.ViewModels.Instances;
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
    public partial class AboutVW : ContentPage
    {
        public AboutVW()
        {
            InitializeComponent();
            BindingContext = new AboutVM();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(BindingContext is AboutVM vm)
            {
                vm.OpenWeblinkUnsuccessful += OnWebLinkFailure;
                vm.OpenEmailUnsuccessful += OnEmailFailure;
            }
        }

        private async void OnWebLinkFailure(object sender, EventArgs e)
        {
            await DisplayAlert(StringResources.Error, StringResources.WebLinkError, StringResources.OK);
        }

        private async void OnEmailFailure(object sender, EventArgs e)
        {
            await DisplayAlert(StringResources.Error, StringResources.WebLinkError, StringResources.OK);
        }

        private void DarkModeToggled(object sender, ToggledEventArgs e)
        {
            if(BindingContext is AboutVM vm)
            {
                if (vm.IsDarkModeEnabled)
                {
                    App.Current.Resources = new DarkTheme();
                    App.AppTheme = Themes.Dark;
                }
                else
                {
                    App.Current.Resources = new LightTheme();
                    App.AppTheme = Themes.Light;
                }
            }
        }
    }
}