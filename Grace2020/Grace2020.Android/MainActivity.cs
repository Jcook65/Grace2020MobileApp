using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.OS;
using PanCardView.Droid;

namespace Grace2020.Droid
{
    [Activity(Label = "Grace2020",
        ScreenOrientation = ScreenOrientation.Portrait, 
        Theme = "@style/SplashTheme", 
        Icon = "@drawable/icon",
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"\t***FATAL UNHANDLED EXCEPTION***: \n\t{args}");
                }
                catch(Exception ex)
                {
                    //this shouldn't happen.
                    System.Diagnostics.Debug.WriteLine($"\tHow did you get here? {ex}");
                }
                finally
                {
                    new AlertDialog.Builder(this).SetMessage("a fatal error occurred").Show();
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    OnDestroy();
                }
            };

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.SetFlags("IndicatorView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CardsViewRenderer.Preserve();

            var config = new FFImageLoading.Config.Configuration
            {
                DiskCacheDuration = new TimeSpan(30, 0, 0, 0),
                ExecuteCallbacksOnUIThread = true
            };
            FFImageLoading.ImageService.Instance.Initialize(config);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            FFImageLoading.Forms.Platform.CachedImageRenderer.InitImageViewHandler();

            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.AddFlags(WindowManagerFlags.LayoutInScreen);

            //ProviderInstaller.InstallIfNeeded(getApplicationContext())
            LoadApplication(new App());
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            System.Diagnostics.Debug.WriteLine($"***INVALIDATING FFIMAGE LOADING IMAGE CACHE DUE TO LOW MEMORY***");
            FFImageLoading.ImageService.Instance.InvalidateMemoryCache();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}