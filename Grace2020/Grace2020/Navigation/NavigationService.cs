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
        public static void GoTo(string uri, ObservableObject bindingContext, bool isAnimated = false)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync(uri, isAnimated);
                Shell.Current.CurrentItem.BindingContext = bindingContext;
            });
        }

        public static void GoTo(string uri, bool isAnimated = false)
        {
            Device.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync(uri, isAnimated));
        }
    }
}
