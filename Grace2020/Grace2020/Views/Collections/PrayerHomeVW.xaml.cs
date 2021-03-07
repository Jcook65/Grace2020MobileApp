using Grace2020.Controls;
using Grace2020.Models.Tables;
using Grace2020.Navigation;
using Grace2020.Utils;
using Grace2020.ViewModels.Collections;
using PanCardView.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Collections
{
    public enum PrayerHomeState : int
    {
        Unexpanded = 1,
        PrayerExpanded = 2,
        RegionExpanded = 3,
        SearchExpanded = 4,
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrayerHomeVW : ThemedContentPage
    {
        private uint _transitionTime = 300;
        private decimal _delayFactor = 1.2m;

        private PrayerHomeState _prayerHomeState;

        public event EventHandler<PrayerHomeState> PrayerHomeStateChanged;

        public PrayerHomeVW()
        {
            InitializeComponent();
            regionImage.CacheKeyFactory = new CustomCacheKeyFactory();
            dayCarousel.IsAutoNavigatingAnimationEnabled = false;
            PrayerHomeStateChanged += OnPrayerHomeStateChanged;
            PrayerHomeStateChanged?.Invoke(this, PrayerHomeState.Unexpanded);
            Device.BeginInvokeOnMainThread(() =>
            {
                imageFrame.IsVisible = false;
                region.Opacity = 0;
                prayer.Opacity = 0;
                searchBox.Opacity = 0;
                backIcon.Opacity = 0;
            });
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (_prayerHomeState == PrayerHomeState.Unexpanded)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    header.TranslationY = 0;
                    region.TranslationY = header.Height;
                    prayer.TranslationY = mainContainer.Height / 1.8;
                    searchBox.TranslationX = mainContainer.Width;
                    searchBox.TranslationY = 0;
                });
            }
        }

        protected override async void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(BindingContext is PrayerHomeVM vm)
            {
                vm.ModulesLookupSelected -= OnModulesLookupSelected;
                vm.ModulesLookupSelected += OnModulesLookupSelected;
                vm.TopicsLoaded -= OnTopicsLoaded;
                vm.TopicsLoaded += OnTopicsLoaded;
                dayCarousel.ItemAppearing += (x, y) =>
                {
                    if (y?.Item != null && BindingContext is PrayerHomeVM bc)
                    {
                        bc.SelectedModulesLookup = y.Item as ModulesLookup;
                    }
                };
                try
                {
                    await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor));
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex, "EXCEPTION IN BINDING CONTEXT CHANGED");
                }
            }
        }

        private async void RegionTapped(object sender, EventArgs e)
        {
            try
            {
                var length = Convert.ToUInt32(_transitionTime * _delayFactor);
                if(_prayerHomeState == PrayerHomeState.RegionExpanded)
                {
                    await UnexpandAnimation(length);
                }
                else
                {
                    await ExpandRegion(length);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION DURING REGION TAPPED");
            }
        }

        private async void PrayerTapped(object sender, EventArgs e)
        {
            try
            {
                var length = Convert.ToUInt32(_transitionTime * _delayFactor);
                if(_prayerHomeState == PrayerHomeState.PrayerExpanded || _prayerHomeState == PrayerHomeState.RegionExpanded)
                {
                    await UnexpandAnimation(length);
                }
                else
                {
                    await ExpandPrayer(length);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION DURING PRAYER TAPPED");
            }
        }

        private async Task ExpandRegion(uint length)
        {
            //if(_prayerHomeState != PrayerHomeState.RegionExpanded)
            //{
                var prayerLowerBound = Device.RuntimePlatform == "iOS"
                    ? mainContainer.Height / 1.3 : mainContainer.Height / 1.15;
                Debug.WriteLine("Entering Expand Region Animation");
                await Task.WhenAll(
                    searchIcon.FadeTo(0, length, Easing.CubicInOut),
                    backIcon.FadeTo(0, length, Easing.CubicInOut),
                    searchBox.TranslateTo(mainContainer.Width, 0, length, Easing.CubicInOut),
                    header.TranslateTo(header.X, -header.Height, length, Easing.CubicInOut),
                    header.FadeTo(0, length, Easing.CubicInOut),
                    region.TranslateTo(region.X, 0, length, Easing.CubicInOut),
                    regionBody.ScaleYTo(2, length, Easing.CubicInOut),
                    prayer.TranslateTo(prayer.X, prayerLowerBound, length, Easing.CubicInOut),
                    prayerBody.ScaleYTo(1, length, Easing.CubicInOut),
                    prayerArrow.ScaleTo(0, length, Easing.CubicInOut),
                    prayerArrow.FadeTo(0, length, Easing.CubicInOut),
                    regionArrow.ScaleTo(1, length, Easing.CubicInOut),
                    regionArrow.FadeTo(1, length, Easing.CubicInOut),
                    regionScrollView.FadeTo(1, length, Easing.CubicInOut));
                PrayerHomeStateChanged?.Invoke(this, PrayerHomeState.RegionExpanded);
                Debug.WriteLine("Exiting Expand Region Animation");
            //}
        }

        private async Task ExpandPrayer(uint length)
        {
            //if(_prayerHomeState != PrayerHomeState.PrayerExpanded)
            //{
                Debug.WriteLine("Entering Expand Prayer Animation");
                await Task.WhenAll(
                    searchIcon.FadeTo(0, length, Easing.CubicInOut),
                    backIcon.FadeTo(0, length, Easing.CubicInOut),
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
                    regionScrollView.ScrollToAsync(0, 0, false)
                    );
                PrayerHomeStateChanged?.Invoke(this, PrayerHomeState.PrayerExpanded);
                Debug.WriteLine("Exiting Expand Prayer Animation");
            //}
        }

        private async Task UnexpandAnimation(uint length)
        {
            Debug.WriteLine("Entering Unexpand Animation");
            await Task.WhenAll(
                searchIcon.FadeTo(1, length, Easing.CubicInOut),
                backIcon.FadeTo(1, length, Easing.CubicInOut),
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
                regionScrollView.FadeTo(1, length, Easing.CubicInOut));
            PrayerHomeStateChanged?.Invoke(this, PrayerHomeState.Unexpanded);
            Debug.WriteLine("Exiting Unexpand Animation");
        }

        private async void SearchIconTapped(object sender, EventArgs e)
        {
            try
            {
                var length = Convert.ToUInt32(_transitionTime * _delayFactor);
                await ExpandSearchBox(length);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION DURING SEARCH ICON TAPPED");
            }
        }

        private async Task ExpandSearchBox(uint length)
        {
            var newHeaderHeight = header.Height + searchBox.Height;
            await Task.WhenAll(
                header.TranslateTo(header.X, searchBox.Height, length, Easing.CubicInOut),
                region.TranslateTo(region.X, newHeaderHeight, length, Easing.CubicInOut),
                prayer.TranslateTo(prayer.X, (mainContainer.Height / 1.8) + searchBox.Height, length, Easing.CubicInOut),
                searchIcon.FadeTo(0, length, Easing.CubicInOut),
                backIcon.FadeTo(0, length, Easing.CubicInOut),
                searchBox.TranslateTo(0, 0, length, Easing.CubicInOut));
            PrayerHomeStateChanged?.Invoke(this, PrayerHomeState.SearchExpanded);
        }

        private async void SearchDismissIconTapped(object sender, EventArgs e)
        {
            try
            {
                await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION DURING SEARCH ICON DISMISS");
            }
        }

        private void BackIconTapped(object sender, EventArgs e)
        {
            NavigationService.GoTo("///Modules");
        }

        private async void OnModulesLookupSelected(object sender, ModulesLookup e)
        {
            try
            {
                if (e != null && BindingContext is PrayerHomeVM vm && vm.Searching)
                {
                    await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor));
                }
                if (!string.IsNullOrWhiteSpace(e?.Topic?.AssetName))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        regionImage.Source = e.Topic.AssetUrl;
                        regionImage.IsVisible = true;
                        imageFrame.IsVisible = true;
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        regionImage.IsVisible = false;
                        imageFrame.IsVisible = false;
                        regionImage.Source = "";
                    });
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION IN MODULES LOOKUP SELECTED");
            }
        }
        private async void OnItemAppearing(PanCardView.CardsView view, PanCardView.EventArgs.ItemAppearingEventArgs args)
        {
            try
            {
                if (_prayerHomeState == PrayerHomeState.SearchExpanded)
                {
                    await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor));
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION IN ITEM APPEARING");
            }
        }

        private async void OnTopicsLoaded(object sender, EventArgs e)
        {
            await Task.WhenAll(
                region.FadeTo(1, easing:Easing.CubicInOut),
                prayer.FadeTo(1, easing: Easing.CubicInOut),
                searchBox.FadeTo(1, easing:Easing.CubicInOut),
                backIcon.FadeTo(1,easing:Easing.CubicInOut));

            dayCarousel.IsAutoNavigatingAnimationEnabled = true;
        }

        private void OnPrayerHomeStateChanged(object sender, PrayerHomeState e)
        {
            _prayerHomeState = e;
            Device.BeginInvokeOnMainThread(() =>
            {
                switch (e)
                {
                    case PrayerHomeState.PrayerExpanded:
                        regionScrollView.IsEnabled = false;
                        prayersList.IsEnabled = true;
                        searchIcon.IsEnabled = false;
                        backIcon.IsEnabled = false;
                        dayCarousel.IsEnabled = false;
                        break;
                    case PrayerHomeState.RegionExpanded:
                        regionScrollView.IsEnabled = true;
                        prayersList.IsEnabled = false;
                        searchIcon.IsEnabled = false;
                        backIcon.IsEnabled = false;
                        dayCarousel.IsEnabled = false;
                        break;
                    case PrayerHomeState.SearchExpanded:
                    case PrayerHomeState.Unexpanded:
                        prayersList.IsEnabled = false;
                        regionScrollView.IsEnabled = false;
                        dayCarousel.IsEnabled = true;
                        searchIcon.IsEnabled = true;
                        backIcon.IsEnabled = true;
                        break;
                }
            });
        }

        private async void OnInfoPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            try
            {
                var prayerLowerBound = Device.RuntimePlatform == "iOS"
                    ? mainContainer.Height / 1.3 : mainContainer.Height / 1.15;
                var length = Convert.ToUInt32(_transitionTime * _delayFactor);
                var delta = Device.RuntimePlatform == "iOS" ? e.TotalY / 12 : e.TotalY;
                switch (e.StatusType)
                {
                    case GestureStatus.Running:
                        region.TranslationY = Math.Min(Math.Max(0, region.TranslationY + delta), 160);

                        var scale = Math.Max(2 - 1.011976 * (1 - Math.Pow(Math.E, -0.0352 * region.TranslationY)), 1);
                        regionBody.ScaleY = Math.Min(2, scale);
                        searchIcon.Opacity = Math.Max(0, region.TranslationY / 160);
                        backIcon.Opacity = Math.Max(0, region.TranslationY / 160);
                        searchBox.TranslationY = Math.Max(0, region.TranslationY - searchBox.TranslationY);
                        header.Opacity = Math.Max(0, region.TranslationY / 160);
                        prayer.TranslationY = Math.Min(prayerLowerBound, Math.Max(-1.529676*region.TranslationY + (prayerLowerBound), mainContainer.Height / 1.8));
                        regionArrow.Opacity = Math.Max(0, Math.Min(-0.0065 * region.TranslationY + 1, 1));
                        regionArrow.Scale = Math.Max(0, Math.Min(-0.0065 * region.TranslationY + 1, 1));

                        break;
                    case GestureStatus.Completed:
                        var bound = _prayerHomeState == PrayerHomeState.Unexpanded ? 150 : 15;
                        if(region.TranslationY > bound)
                        {
                            await UnexpandAnimation(length);
                        }
                        else
                        {
                            await ExpandRegion(length);
                        }
                        Debug.WriteLine("Info Pan Completed");
                        break;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION WHILE INFO PANNING");
            }
        }

        private async void OnPrayerPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            try
            {
                var length = Convert.ToUInt32(_transitionTime * _delayFactor);
                var prayerStart = mainContainer.Height / 1.8;
                var prayerEnd =  Device.RuntimePlatform == "iOS"
                    ? mainContainer.Height / 1.3 : mainContainer.Height / 1.15;
                var delta = Device.RuntimePlatform == "iOS" ? e.TotalY / 12 : e.TotalY;
                switch (e.StatusType)
                {
                    case GestureStatus.Running:
                        prayer.TranslationY = Math.Min(Math.Max(0, prayer.TranslationY + delta), prayerEnd);
                        if(prayer.TranslationY < 450)
                        {
                            region.TranslationY = Math.Min(Math.Max(-100, region.TranslationY + delta), 160);
                        }

                        var scale = Math.Max(2 - 1.011976 * (1 - Math.Pow(Math.E, -0.0352 * prayer.TranslationY)), 1);
                        prayerBody.ScaleY = Math.Min(2, scale);
                        searchIcon.Opacity = Math.Max(0, Math.Min(prayer.TranslationY / prayerStart, 1));
                        backIcon.Opacity = Math.Max(0, Math.Min(prayer.TranslationY / prayerStart, 1));
                        regionScrollView.Opacity = Math.Max(0, Math.Min(prayer.TranslationY / prayerStart, 1));
                        searchBox.TranslationY = Math.Max(0, prayer.TranslationY - searchBox.TranslationY);
                        header.Opacity = Math.Max(0, prayer.TranslationY / prayerStart);
                        prayerArrow.Opacity = Math.Max(0, Math.Min(-0.0065 * prayer.TranslationY + 1, 1));
                        prayerArrow.Scale = Math.Max(0, Math.Min(-0.0065 * prayer.TranslationY + 1, 1));

                        Debug.WriteLine("prayer translation Y: " + prayer.TranslationY);
                        break;
                    case GestureStatus.Completed:
                        var upperBound = _prayerHomeState == PrayerHomeState.Unexpanded ? 410 : 35;
                        var lowerBound = _prayerHomeState == PrayerHomeState.Unexpanded ? 475 : 570;
                        if (prayer.TranslationY > upperBound && prayer.TranslationY < lowerBound)
                        {
                            await UnexpandAnimation(length);
                        }
                        else if(prayer.TranslationY <= upperBound)
                        {
                            await ExpandPrayer(length);
                        }
                        else
                        {
                            await ExpandRegion(length);
                        }

                        Debug.WriteLine("Prayer Pan Completed");
                        break;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION WHILE PRAYER PANNING");
            }
        }
    }
}