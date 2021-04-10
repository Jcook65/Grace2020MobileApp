using Grace2020.ViewModels.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Collections
{
    [QueryProperty("IsBackNavEnabled", "isBackNavEnabled")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModulesVW : ContentPage
    {
        private string _isBackNavEnabled;
        public string IsBackNavEnabled
        {
            get { return _isBackNavEnabled; }
            set { _isBackNavEnabled = value; BackNavPropertyChanged?.Invoke(this, _isBackNavEnabled); }
        }

        public event EventHandler<string> BackNavPropertyChanged;

        public ModulesVW()
        {
            InitializeComponent();
            BackNavPropertyChanged += (s, e) =>
            {
                if (BindingContext is ModulesVM vm && bool.TryParse(_isBackNavEnabled, out bool isBackEnabled))
                {
                    vm.IsBackNavEnabled = isBackEnabled;
                }
            };
            BindingContext = new ModulesVM();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if(BindingContext is ModulesVM vm)
            {
                await vm.Load();
                if (!vm.IsBackNavEnabled)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await logo.FadeTo(1, length: 1000, easing: Easing.CubicOut);
                        await Task.WhenAll(
                            greetingEN.FadeTo(1, length: 500, easing: Easing.CubicInOut),
                            greetingJA.FadeTo(1, length: 500, easing: Easing.CubicInOut),
                            date.FadeTo(1, length: 1000, Easing.CubicInOut));
                        await Task.WhenAll(
                            modulesList.FadeTo(1, length: 500, Easing.CubicInOut),
                            jumpBackIn.FadeTo(1, length: 500, Easing.CubicInOut));
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.WhenAll(
                            greetingEN.FadeTo(1, length: 250, easing: Easing.CubicInOut),
                            greetingJA.FadeTo(1, length: 250, easing: Easing.CubicInOut),
                            logo.FadeTo(1, length: 250, easing: Easing.CubicInOut),
                            date.FadeTo(1, length: 250, Easing.CubicInOut),
                            modulesList.FadeTo(1, length: 250, Easing.CubicInOut),
                            jumpBackIn.FadeTo(1, length: 250, Easing.CubicInOut));
                    });
                }
            }
        }
    }
}