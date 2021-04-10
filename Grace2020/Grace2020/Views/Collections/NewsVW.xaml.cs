using Grace2020.ViewModels.Abstractions;
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
    public partial class NewsVW : ContentPage
    {
        public NewsVW()
        {
            InitializeComponent();
            Device.BeginInvokeOnMainThread(() => newsList.Opacity = 0);
            BindingContext = new NewsVM();
            newsList.ItemSelected += (e, sender) =>
            {
                newsList.SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            newsList.Opacity = 0;
            if(BindingContext is NewsVM vm)
            {
                vm.Loaded += OnLoaded;
            }
            Task.Run(async () => await (BindingContext as NewsVM)?.Load());
        }

        private void OnLoaded(object sender, SuccessEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () => await newsList.FadeTo(1, easing: Easing.CubicInOut));
        }
    }
}