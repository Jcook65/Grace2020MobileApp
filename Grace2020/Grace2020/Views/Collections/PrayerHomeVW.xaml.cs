﻿using Grace2020.Controls;
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
        private bool _initialLoad;

        private PrayerHomeState _prayerHomeState;

        public event EventHandler<PrayerHomeState> PrayerHomeStateChanged;

        public PrayerHomeVW()
        {
            InitializeComponent();
            _initialLoad = true; //this is dumb
            regionImage.CacheKeyFactory = new CustomCacheKeyFactory();
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

            dayCarousel.ItemAppeared += (sender, e) => 
            {
                if(BindingContext is PrayerHomeVM vm && !_initialLoad)
                {
                    vm.SelectedModulesLookup = dayCarousel.SelectedItem as ModulesLookup;
                }
                else if(_initialLoad)
                {
                    _initialLoad = false;
                }
            };
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
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
                try
                {
                    await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor), Easing.CubicInOut);
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
                    await UnexpandAnimation(length, Easing.CubicInOut);
                }
                else
                {
                    await ExpandRegion(length, Easing.CubicInOut);
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
                    await UnexpandAnimation(length, Easing.CubicInOut);
                }
                else
                {
                    await ExpandPrayer(length, Easing.CubicInOut);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION DURING PRAYER TAPPED");
            }
        }

        private async Task ExpandRegion(uint length, Easing easing)
        {
            var prayerLowerBound = Device.RuntimePlatform == "iOS"
                ? mainContainer.Height / 1.3 : mainContainer.Height / 1.15;
            Debug.WriteLine("Entering Expand Region Animation");
            await Task.WhenAll(
                searchIcon.FadeTo(0, length, easing),
                backIcon.FadeTo(0, length, easing),
                searchBox.TranslateTo(mainContainer.Width, 0, length, easing),
                header.FadeTo(0, length, easing),
                region.TranslateTo(region.X, -regionTab.Height, length, easing),
                prayer.TranslateTo(prayer.X, prayerLowerBound, length, easing),
                prayerArrow.ScaleTo(0, length, easing),
                prayerArrow.FadeTo(0, length, easing),
                regionArrow.ScaleTo(1, length, easing),
                regionArrow.FadeTo(1, length, easing),
                regionScrollView.FadeTo(1, length, easing),
                prayerMask.FadeTo(1, length, easing),
                imageFrame.FadeTo(1, length, easing),
                regionText.TranslateTo(regionText.X, 50, length, easing),
                prayerText.TranslateTo(prayerText.X, 0, length, easing));
            PrayerHomeStateChanged?.Invoke(this, PrayerHomeState.RegionExpanded);
            Debug.WriteLine("Exiting Expand Region Animation");
        }

        private async Task ExpandPrayer(uint length, Easing easing)
        { 
            Debug.WriteLine("Entering Expand Prayer Animation");
            await Task.WhenAll(
                searchIcon.FadeTo(0, length, easing),
                backIcon.FadeTo(0, length, easing),
                searchBox.TranslateTo(mainContainer.Width, 0, length, easing),
                header.FadeTo(0, length, easing),
                region.TranslateTo(region.X, -region.Height, length, easing),
                prayer.TranslateTo(prayer.X, -prayerTab.Height - 10, length, easing),
                prayerArrow.ScaleTo(1, length, easing),
                prayerArrow.FadeTo(1, length, easing),
                regionArrow.ScaleTo(0, length, easing),
                regionArrow.FadeTo(0, length, easing),
                regionScrollView.ScrollToAsync(0, 0, false),
                prayerMask.FadeTo(0, length, easing),
                imageFrame.FadeTo(0, length, easing),
                prayerText.TranslateTo(prayerText.X, 50, length, easing),
                regionText.TranslateTo(regionText.X, 0, length, easing));
            PrayerHomeStateChanged?.Invoke(this, PrayerHomeState.PrayerExpanded);
            Debug.WriteLine("Exiting Expand Prayer Animation");
        }

        private async Task UnexpandAnimation(uint length, Easing easing)
        {
            Debug.WriteLine("Entering Unexpand Animation");
            await Task.WhenAll(
                    searchIcon.FadeTo(1, length, easing),
                    backIcon.FadeTo(1, length, easing),
                    searchBox.TranslateTo(mainContainer.Width, 0, length, easing),
                    header.TranslateTo(header.X, 0, length, Easing.CubicInOut),
                    header.FadeTo(1, length, easing),
                    region.TranslateTo(region.X, header.Height, length, easing),
                    prayer.TranslateTo(prayer.X, mainContainer.Height / 1.8, length, easing),
                    prayerArrow.ScaleTo(0, length, easing),
                    prayerArrow.FadeTo(0, length, easing),
                    regionArrow.ScaleTo(0, length, easing),
                    regionArrow.FadeTo(0, length, easing),
                    regionScrollView.ScrollToAsync(0, 0, false),
                    regionScrollView.FadeTo(1, length, easing),
                    prayerMask.FadeTo(0, length, easing),
                    imageFrame.FadeTo(0, length, easing),
                    prayerText.TranslateTo(prayerText.X, 0, length, easing),
                    regionText.TranslateTo(regionText.X, 0, length, easing));
            PrayerHomeStateChanged?.Invoke(this, PrayerHomeState.Unexpanded);
            Debug.WriteLine("Exiting Unexpand Animation");

        }

        private void SearchIconTapped(object sender, EventArgs e)
        {
            try
            {
                AppShell.Current.Navigating += OnNavigating;
                NavigationService.GoTo(ViewLocator.TopicsPage);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION DURING SEARCH ICON TAPPED");
            }
        }

        private void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            if(e.Target.Location.OriginalString == "..")
            {
                if(BindingContext is PrayerHomeVM vm)
                {
                    Device.BeginInvokeOnMainThread(() => dayCarousel.SelectedIndex = vm.SelectedModulesLookup?.Sequence - 1 ?? 0);
                }

                AppShell.Current.Navigating -= OnNavigating;
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
                await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor), Easing.CubicInOut);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION DURING SEARCH ICON DISMISS");
            }
        }

        private void BackIconTapped(object sender, EventArgs e)
        {
            NavigationService.GoTo($"{ViewLocator.ModulesPage}?isBackNavEnabled=true");
        }

        private async void OnModulesLookupSelected(object sender, ModulesLookup e)
        {
            try
            {
                if (e != null && BindingContext is PrayerHomeVM vm && vm.Searching)
                {
                    await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor), Easing.CubicInOut);
                }

                if (BindingContext is PrayerHomeVM x && dayCarousel.SelectedIndex != x.SelectedModulesLookup?.Sequence - 1)
                {
                    Device.BeginInvokeOnMainThread(() => dayCarousel.SelectedIndex = x.SelectedModulesLookup?.Sequence - 1 ?? 0);
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
                    await UnexpandAnimation(Convert.ToUInt32(_transitionTime * _delayFactor), Easing.CubicInOut);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION IN ITEM APPEARING");
            }
        }

        private async void OnTopicsLoaded(object sender, EventArgs e)
        {
            try
            {
                await Task.WhenAll(
                    region.FadeTo(1, easing:Easing.CubicInOut),
                    prayer.FadeTo(1, easing: Easing.CubicInOut),
                    searchBox.FadeTo(1, easing:Easing.CubicInOut),
                    backIcon.FadeTo(1,easing:Easing.CubicInOut));

                //if (BindingContext is PrayerHomeVM vm)
                //{
                //    Device.BeginInvokeOnMainThread(() => dayCarousel.SelectedIndex = vm.SelectedModulesLookup?.Sequence - 1 ?? 0);
                //}
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex, "EXCEPTION IN TOPICS LOADED");
            }
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

                //if(BindingContext is PrayerHomeVM vm)
                //{
                //    dayCarousel.Position = vm.SelectedModulesLookup?.Sequence - 1 ?? 0;
                //}
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
                        region.TranslationY = Math.Min(Math.Max(-regionTab.Height, region.TranslationY + delta), 160);
                        prayerMask.Opacity = Math.Max(0, Math.Min(region.TranslationY / 160, 1));
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
                            await UnexpandAnimation(length, Easing.CubicOut);
                        }
                        else
                        {
                            await ExpandRegion(length, Easing.CubicOut);
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
                        prayer.TranslationY = Math.Min(Math.Max(-prayerTab.Height - 10, prayer.TranslationY + delta), prayerEnd);
                        if(prayer.TranslationY < 450)
                        {
                            region.TranslationY = Math.Min(Math.Max(-(regionTab.Height*3), region.TranslationY + delta), 160);
                        }
                        searchIcon.Opacity = Math.Max(0, Math.Min(prayer.TranslationY / prayerStart, 1));
                        prayerMask.Opacity = Math.Max(0, Math.Min(prayer.TranslationY / prayerStart, 1));
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
                            await UnexpandAnimation(length, Easing.CubicOut);
                        }
                        else if(prayer.TranslationY <= upperBound)
                        {
                            await ExpandPrayer(length, Easing.CubicOut);
                        }
                        else
                        {
                            await ExpandRegion(length, Easing.CubicOut);
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