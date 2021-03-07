using Grace2020.Models.Tables;
using Grace2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Instances
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventItemVW : ContentView
    {
        public EventItemVW()
        {
            InitializeComponent();
            eventImage.CacheKeyFactory = new CustomCacheKeyFactory();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            eventImage.Source = null;
            if(BindingContext is Event evt)
            {
                Device.BeginInvokeOnMainThread(() => eventImage.Source = evt.EventImage?.AssetUrl);
            }
        }
    }
}