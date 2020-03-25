using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Grace2020;
using Grace2020.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(AppShell), typeof(ShellRenderer_iOS))]
namespace Grace2020.iOS.Renderers
{
    public class ShellRenderer_iOS : ShellRenderer
    {
        protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
        {
            return new CustomTabbarAppearance();
        }
    }

    public class CustomTabbarAppearance : ShellTabBarAppearanceTracker
    {
        public override void UpdateLayout(UITabBarController controller)
        {
            base.UpdateLayout(controller);
            UITabBar myTabBar = controller.TabBar;
            foreach(var child in myTabBar.Items)
            {
                child.ImageInsets = new UIEdgeInsets(10, 0, -10, 0);
            }
        }
    }
}