using Grace2020.Resources;
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
    public partial class RoundedNewsImageVW : ContentView
    {
        public RoundedNewsImageVW()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            newsImage.Source = null;
            if (BindingContext is SelectableItemVM item && item.Item is string source)
            {
                newsImage.Source = string.Format(Constants.GRACE2020NewsImageUrl, source);
            }
        }
    }
}