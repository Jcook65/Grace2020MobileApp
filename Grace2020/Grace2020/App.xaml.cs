using Xamarin.Forms;
using Grace2020.Resources;
using System.Reflection;
using Grace2020.Language;
using Grace2020.Utils;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Grace2020.DependencyInjection;
using Grace2020.Services;
using System.Globalization;
using Grace2020.Navigation;
using Grace2020.Views.Collections;
using Grace2020.Views.Instances;
using System.ComponentModel;

namespace Grace2020
{
    public enum Themes
    {
        Light = 0,
        Dark = 1
    }

    public partial class App : Application, INotifyPropertyChanged
    {
        public static Themes AppTheme { get; set; }

        public App()
        {
            InitializeComponent();

            System.Diagnostics.Debug.WriteLine("====== resource debug info =========");
            var assembly = typeof(App).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            System.Diagnostics.Debug.WriteLine("====================================");

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var local = DependencyService.Get<ILocalize>();
                var ci = local.GetCurrentCultureInfo();
                StringResources.Culture = ci;
                local.SetLocale(ci);
            }

            RegisterDependencies();

            RegisterPages();

            Task.Run(async () => await DbUtil.InitDbAsync(new string[] { "Grace2020.Models.Tables" }));

            MainPage = new AppShell();
        }

        public static CultureInfo GetCultureInfo()
        {
            var local = DependencyService.Get<ILocalize>();
            return local.GetCurrentCultureInfo();
        }

        protected override void OnStart()
        {
            //// Handle when your app starts
            //TODO: Pull Data from servers
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            //TODO: Pull Data from servers
        }

        private void RegisterPages()
        {
            Routing.RegisterRoute(ViewLocator.DayPicker, typeof(DayPickerVW));
            Routing.RegisterRoute(ViewLocator.NewsDetailPage, typeof(NewsDetailsVW));
        }
        private void RegisterDependencies()
        {

        }
    }
}
