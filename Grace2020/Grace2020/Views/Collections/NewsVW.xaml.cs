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
            BindingContext = new NewsVM();
            (BindingContext as NewsVM).Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, SuccessEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => InvalidateMeasure());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as NewsVM)?.Load();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}