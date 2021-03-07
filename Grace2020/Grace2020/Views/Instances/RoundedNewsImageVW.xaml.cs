using FFImageLoading;
using FFImageLoading.Forms;
using Grace2020.Models.Tables;
using Grace2020.Resources;
using Grace2020.Utils;
using Grace2020.ViewModels.Instances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Instances
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoundedNewsImageVW : ContentView
    {
        public RoundedNewsImageVW()
        {
            InitializeComponent();
            newsImage.CacheKeyFactory = new Utils.CustomCacheKeyFactory();
        }

        public CachedImage GetImage()
        {
            return newsImage;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            newsImage.Source = null;
            if (BindingContext is SelectableItemVM item && item.Item is NewsAssetLookup source)
            {
                Device.BeginInvokeOnMainThread(() => newsImage.Source = source.AssetUrl);
            }
        }
    }
}