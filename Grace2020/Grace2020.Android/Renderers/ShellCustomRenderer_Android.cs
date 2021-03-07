using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using Grace2020;
using Grace2020.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AppShell), typeof(ShellCustomRenderer_Android))]
namespace Grace2020.Droid.Renderers
{
    public class ShellCustomRenderer_Android : ShellRenderer
    {

        public ShellCustomRenderer_Android(Context context)
            : base(context)
        {

        }

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new MarginedTabBarAppearance(this, shellItem);
        }
    }

    public class MarginedTabBarAppearance : ShellBottomNavViewAppearanceTracker
    {

        public MarginedTabBarAppearance(IShellContext context, ShellItem shellItem)
            : base(context, shellItem)
        {

        }
        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);
            bottomView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityUnlabeled;
        }
    }
}