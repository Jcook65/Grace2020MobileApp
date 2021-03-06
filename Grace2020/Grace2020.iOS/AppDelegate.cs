﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using PanCardView.iOS;
using UIKit;

namespace Grace2020.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.SetFlags("IndicatorView_Experimental");
            Xamarin.Forms.Forms.Init();
            CardsViewRenderer.Preserve();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.InitImageSourceHandler();
            var config = new FFImageLoading.Config.Configuration
            {
                DiskCacheDuration = new TimeSpan(30, 0, 0, 0),
                ExecuteCallbacksOnUIThread = true
            };
            FFImageLoading.ImageService.Instance.Initialize(config);

            LoadApplication(new App());
            UINavigationBar.Appearance.Translucent = false;
            return base.FinishedLaunching(app, options);
        }
    }
}
