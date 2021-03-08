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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModulesVW : ContentPage
    {
        public ModulesVW()
        {
            InitializeComponent();
            BindingContext = new ModulesVM();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if(BindingContext is ModulesVM vm)
            {
                await vm.Load();
            }

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
    }
}