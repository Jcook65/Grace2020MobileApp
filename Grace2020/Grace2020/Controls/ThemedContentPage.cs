using Grace2020.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Grace2020.Controls
{
    public class ThemedContentPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetPageTheme();
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
            GetPageTheme();
        }

        public void GetPageTheme()
        {
            if(BindingContext is VMBase vm)
            {
                vm.AppTheme = (int)App.AppTheme;
            }
        }
    }
}
