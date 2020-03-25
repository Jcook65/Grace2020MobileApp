using Grace2020.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Grace2020.Controls
{
    public class ThemedContentPage : ContentPage
    {
        public void ChangePageTheme()
        {
            if(BindingContext is VMBase vm)
            {
                vm.AppTheme = (int)App.AppTheme;
            }
        }
    }
}
