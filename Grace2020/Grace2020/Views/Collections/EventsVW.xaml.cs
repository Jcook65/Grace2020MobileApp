using Grace2020.Models.Tables;
using Grace2020.Resources;
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
    public partial class EventsVW : ContentPage
    {
        public EventsVW()
        {
            InitializeComponent();
            Device.BeginInvokeOnMainThread(() => eventList.Opacity = 0);
            BindingContext = new EventsVM();
            eventList.ItemSelected += (e, sender) =>
            {
                eventList.SelectedItem = null;
            };
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(BindingContext is EventsVM vm)
            {
                vm.OpenWeblinkUnsuccessful += OnWeblinkFailure;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            eventList.Opacity = 0;
            if (BindingContext is EventsVM vm)
            {
                vm.Loaded += OnLoaded;
                Task.Run(async () => await vm.Load());
            }
        }

        private void OnLoaded(object sender, SuccessEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () => await eventList.FadeTo(1, easing: Easing.CubicInOut));
        }

        private void OnWeblinkFailure(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert(StringResources.Error, StringResources.WebLinkError, StringResources.OK));
        }

        private void eventList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if(BindingContext is EventsVM vm && e.Item is Event et)
            {
                vm.OpenWebsite.Execute(et.URL);
            }
        }
    }
}