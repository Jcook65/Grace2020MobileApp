using Grace2020.Controls;
using Grace2020.Models.Tables;
using Grace2020.ViewModels.Collections;
using PanCardView.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Collections
{
    enum PrayerHomeState : int
    {
        Unexpanded = 1,
        PrayerExpanded = 2,
        RegionExpanded = 3,
        ModulesExpanded = 4,
        SearchExpanded = 5,
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrayerHomeVW : ThemedContentPage
    {
        private uint _transitionTime = 300;
        private decimal _delayFactor = 1.2m;
        private bool _isSearchVisible;

        private PrayerHomeState _prayerHomeState = PrayerHomeState.Unexpanded;

        public PrayerHomeVW()
        {
            InitializeComponent();
            BindingContext = new PrayerHomeVM();
            prayerArrow.Scale = 0;
            prayerArrow.Opacity = 0;
            regionArrow.Scale = 0;
            regionArrow.Scale = 0;
            regionScrollView.IsEnabled = false;
            prayersList.IsEnabled = false;

            prayersList.ItemSelected += (sender, e) =>
            {
                (sender as ListView).SelectedItem = null;
            };
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (_prayerHomeState == PrayerHomeState.Unexpanded)
            {
                header.TranslationY = 0;
                region.TranslationY = header.Height;
                prayer.TranslationY = mainContainer.Height/1.8;
                modulesList.TranslationY = -modulesList.Height;
                modulesList.IsEnabled = false;
                searchBox.TranslationX = mainContainer.Width;
                searchBox.TranslationY = 0;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PrayerHomeVM bindingContext)
            {
                Device.BeginInvokeOnMainThread(async () => await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor)));
                bindingContext.ModulesLookupSelected -= OnModulesLookupSelected;
                bindingContext.ModulesLookupSelected += OnModulesLookupSelected;
                await bindingContext.Load();
            }
        }

        private async void RegionTapped(object sender, EventArgs e)
        {
            var length = Convert.ToUInt32(_transitionTime * _delayFactor);
            if(_prayerHomeState == PrayerHomeState.RegionExpanded)
            {
                await UnexpandAnimation(length);
            }
            else
            {

                Debug.WriteLine("Entering Expand Region Animation");
                await Task.WhenAll(
                    searchIcon.FadeTo(0, length, Easing.CubicInOut),
                    searchBox.TranslateTo(mainContainer.Width, 0, length, Easing.CubicInOut),
                    header.TranslateTo(header.X, -header.Height, length, Easing.CubicInOut),
                    header.FadeTo(0, length, Easing.CubicInOut),
                    region.TranslateTo(region.X, 0, length, Easing.CubicInOut),
                    regionBody.ScaleYTo(2, length, Easing.CubicInOut),
                    prayer.TranslateTo(prayer.X, mainContainer.Height / 1.23, length, Easing.CubicInOut),
                    prayerBody.ScaleYTo(1, length, Easing.CubicInOut),
                    prayerArrow.ScaleTo(0, length, Easing.CubicInOut),
                    prayerArrow.FadeTo(0, length, Easing.CubicInOut),
                    regionArrow.ScaleTo(1, length, Easing.CubicInOut),
                    regionArrow.FadeTo(1, length, Easing.CubicInOut),
                    modulesList.TranslateTo(modulesList.X, -modulesList.Height, length, Easing.CubicInOut));
                _prayerHomeState = PrayerHomeState.RegionExpanded;
                regionScrollView.IsEnabled = true;
                prayersList.IsEnabled = false;
                modulesList.IsEnabled = false;
                searchIcon.IsEnabled = false;
                dayCarousel.IsEnabled = false;
                Debug.WriteLine("Exiting Expand Region Animation");
            }
        }

        private async void PrayerTapped(object sender, EventArgs e)
        {
            var length = Convert.ToUInt32(_transitionTime * _delayFactor);
            if(_prayerHomeState == PrayerHomeState.PrayerExpanded)
            {
                await UnexpandAnimation(length);
            }
            else
            {
                Debug.WriteLine("Entering Expand Prayer Animation");

                await Task.WhenAll(
                    searchIcon.FadeTo(0, length, Easing.CubicInOut),
                    searchBox.TranslateTo(mainContainer.Width, 0, length, Easing.CubicInOut),
                    header.TranslateTo(header.X, -header.Height, length, Easing.CubicInOut),
                    header.FadeTo(0, length, Easing.CubicInOut),
                    region.TranslateTo(region.X, -region.Height, length, Easing.CubicInOut),
                    regionBody.ScaleYTo(1, length, Easing.CubicInOut),
                    prayer.TranslateTo(prayer.X, 0, length, Easing.CubicInOut),
                    prayerBody.ScaleYTo(2, length, Easing.CubicInOut),
                    prayerArrow.ScaleTo(1, length, Easing.CubicInOut),
                    prayerArrow.FadeTo(1, length, Easing.CubicInOut),
                    regionArrow.ScaleTo(0, length, Easing.CubicInOut),
                    regionArrow.FadeTo(0, length, Easing.CubicInOut),
                    regionScrollView.ScrollToAsync(0, 0, false),
                    modulesList.TranslateTo(modulesList.X, -modulesList.Height, length, Easing.CubicInOut));
                _prayerHomeState = PrayerHomeState.PrayerExpanded;
                regionScrollView.IsEnabled = false;
                prayersList.IsEnabled = true;
                modulesList.IsEnabled = false;
                searchIcon.IsEnabled = false;
                dayCarousel.IsEnabled = false;

                Debug.WriteLine("Exiting Expand Prayer Animation");
            }
        }

        private async Task UnexpandAnimation(uint length)
        {
            Debug.WriteLine("Entering Unexpand Animation");
            searchIcon.IsEnabled = true;
            await Task.WhenAll(
                searchIcon.FadeTo(1, length, Easing.CubicInOut),
                searchBox.TranslateTo(mainContainer.Width, 0, length, Easing.CubicInOut),
                header.TranslateTo(header.X, 0, length, Easing.CubicInOut),
                header.FadeTo(1, length, Easing.CubicInOut),
                region.TranslateTo(region.X, header.Height, length, Easing.CubicInOut),
                regionBody.ScaleYTo(1, length, Easing.CubicInOut),
                prayer.TranslateTo(prayer.X, mainContainer.Height / 1.8, length, Easing.CubicInOut),
                prayerBody.ScaleYTo(1, length, Easing.CubicInOut),
                prayerArrow.ScaleTo(0, length, Easing.CubicInOut),
                prayerArrow.FadeTo(0, length, Easing.CubicInOut),
                regionArrow.ScaleTo(0, length, Easing.CubicInOut),
                regionArrow.FadeTo(0, length, Easing.CubicInOut),
                regionScrollView.ScrollToAsync(0, 0, false),
                modulesList.TranslateTo(modulesList.X, -modulesList.Height, length, Easing.CubicInOut));
            _prayerHomeState = PrayerHomeState.Unexpanded;
            prayersList.IsEnabled = false;
            regionScrollView.IsEnabled = false;
            modulesList.IsEnabled = false;
            _isSearchVisible = false;
            dayCarousel.IsEnabled = true;

            Debug.WriteLine("Exiting Unexpand Animation");
        }

        private async void HeaderTapped(object sender, EventArgs e)
        {
            if(BindingContext is PrayerHomeVM vm)
            {                
                modulesCarousel.ScrollTo(vm.SelectedModule);
            }

            modulesList.IsEnabled = true;
            searchIcon.IsEnabled = false;
            var length = Convert.ToUInt32(_transitionTime * _delayFactor);

            Debug.WriteLine("Entering Expand Modules Animation");

            await Task.WhenAll(
                searchIcon.FadeTo(0, length, Easing.CubicInOut),
                searchBox.TranslateTo(mainContainer.Width, 0, length, Easing.CubicInOut),
                header.TranslateTo(header.X, 0, length, Easing.CubicInOut),
                header.FadeTo(0, length, Easing.CubicInOut),
                region.TranslateTo(region.X, modulesList.Height, length, Easing.CubicInOut),
                regionBody.ScaleYTo(1, length, Easing.CubicInOut),
                prayer.TranslateTo(prayer.X, mainContainer.Height / 1.8, length, Easing.CubicInOut),
                prayerBody.ScaleYTo(1, length, Easing.CubicInOut),
                prayerArrow.ScaleTo(0, length, Easing.CubicInOut),
                prayerArrow.FadeTo(0, length, Easing.CubicInOut),
                regionArrow.ScaleTo(0, length, Easing.CubicInOut),
                regionArrow.FadeTo(0, length, Easing.CubicInOut),
                regionScrollView.ScrollToAsync(0, 0, false),
                modulesList.TranslateTo(modulesList.X, 0, length, Easing.CubicInOut));
            _prayerHomeState = PrayerHomeState.ModulesExpanded;
            prayersList.IsEnabled = false;
            regionScrollView.IsEnabled = false;
            dayCarousel.IsEnabled = false;

            Debug.WriteLine("Exiting Expand Modules Animation");
        }

        private async void ModuleArrowTapped(object sender, EventArgs e)
        {
            if(BindingContext is PrayerHomeVM vm)
            {
                vm.SelectedModule = modulesCarousel.CurrentItem as Module;
            }
            await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor));
        }

        private async void SearchIconTapped(object sender, EventArgs e)
        {
            var newHeaderHeight = header.Height + searchBox.Height;
            var length = Convert.ToUInt32(_transitionTime * _delayFactor);
            _isSearchVisible = true;
            await Task.WhenAll(
                header.TranslateTo(header.X, searchBox.Height, length, Easing.CubicInOut),
                region.TranslateTo(region.X, newHeaderHeight,  length, Easing.CubicInOut),
                prayer.TranslateTo(prayer.X, (mainContainer.Height / 1.8) + searchBox.Height, length, Easing.CubicInOut),
                searchIcon.FadeTo(0, length, Easing.CubicInOut),
                searchBox.TranslateTo(0, 0, length, Easing.CubicInOut));
            dayCarousel.IsEnabled = true;
            _prayerHomeState = PrayerHomeState.SearchExpanded;
        }

        private async void SearchDismissIconTapped(object sender, EventArgs e)
        {
            await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor));
        }

        private void OnModulesLookupSelected(object sender, ModulesLookup e)
        {
            if (e != null && BindingContext is PrayerHomeVM vm && vm.Searching)
            {
                Device.BeginInvokeOnMainThread(async() => await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor)));
            }
            Device.BeginInvokeOnMainThread(() => regionImage.Source = e?.ImageURL);
        }

        private async void OnItemAppearing(PanCardView.CardsView view, PanCardView.EventArgs.ItemAppearingEventArgs args)
        {
            if(_prayerHomeState == PrayerHomeState.SearchExpanded)
            {
                await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor));
            }
        }

        //private void DayCarouselPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        //{
        //    var opacity = 20 / Math.Abs(dayCarousel.CurrentDiff);

        //    if (dayCarousel?.CurrentView != null && dayCarousel.CurrentView.Opacity != opacity)
        //    {
        //        dayCarousel.CurrentView.Opacity = opacity;
        //    }
        //}
    }
}