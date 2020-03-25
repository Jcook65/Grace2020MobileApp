using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Grace2020.Navigation
{
    public class NavigationService
    {
        public static async Task GoToAsync(string uri, ObservableObject bindingContext, bool isAnimated = false)
        {
            await Shell.Current.GoToAsync(uri, isAnimated);
            Shell.Current.CurrentItem.BindingContext = bindingContext;
        }
    }
}
