using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Grace2020.Controls;
using Grace2020.iOS.Renderers;
using Grace2020.Styles;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(ContentPage), typeof(PageRenderer_iOS))]
namespace Grace2020.iOS.Renderers
{
    public class PageRenderer_iOS : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if(e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                SetAppTheme();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\tERROR CHANGING THEME: {ex}");
            }
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);
            if(previousTraitCollection != null && TraitCollection.UserInterfaceStyle != previousTraitCollection.UserInterfaceStyle)
            {
                SetAppTheme();
                if(Element is ThemedContentPage page)
                {
                    page.GetPageTheme();
                }
            }
        }

        private void SetAppTheme()
        {
            if(TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
            {
                if (App.AppTheme == Themes.Dark)
                    return;
                App.Current.Resources = new DarkTheme();
                App.AppTheme = Themes.Dark;
            }
            else
            {
                if (App.AppTheme != Themes.Dark)
                    return;
                App.Current.Resources = new LightTheme();
                App.AppTheme = Themes.Light;
            }
        }
    }
}