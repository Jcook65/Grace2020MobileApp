using Grace2020.Models.Tables;
using Grace2020.Resources;
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
            BindingContext = new EventsVM();
            eventList.ItemSelected += (e, sender) =>
            {
                eventList.SelectedItem = null;
            };
        }

        protected override async void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(BindingContext is EventsVM vm)
            {
                await vm.Load();
                vm.OpenWeblinkUnsuccessful += OnWeblinkFailure;
            }
        }

        private async void OnWeblinkFailure(object sender, EventArgs e)
        {
            await DisplayAlert(StringResources.Error, StringResources.WebLinkError, StringResources.OK);
        }

        private void eventList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if(BindingContext is EventsVM vm && e.Item is Event et)
            {
                vm.OpenWebsite.Execute(et.Weblink);
            }
        }
    }
}