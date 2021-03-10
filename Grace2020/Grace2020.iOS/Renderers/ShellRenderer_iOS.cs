using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
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
        public override void SetAppearance(UITabBarController controller, ShellAppearance appearance)
        {
            UITabBar myTabBar = controller.TabBar;
            base.SetAppearance(controller, appearance);
            if (myTabBar.Items != null && myTabBar.Items.Count() == 4)
            {
                UITabBarItem item = myTabBar.Items[0];

                item.Image = UIImage.FromBundle("HomeIcon.png");
                item.SelectedImage = UIImage.FromBundle("HomeIcon.png");
                item.Image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.SelectedImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.ImageInsets = new UIEdgeInsets(5, 0, -5, 0);

                item = myTabBar.Items[1];

                item.Image = UIImage.FromBundle("NewsIcon.png");
                item.SelectedImage = UIImage.FromBundle("NewsIcon.png");
                item.Image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.SelectedImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.ImageInsets = new UIEdgeInsets(5, 0, -5, 0);

                item = myTabBar.Items[2];

                item.Image = UIImage.FromBundle("EventsIcon.png");
                item.SelectedImage = UIImage.FromBundle("EventsIcon.png");
                item.Image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.SelectedImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.ImageInsets = new UIEdgeInsets(5, 0, -5, 0);


                item = myTabBar.Items[3];

                item.Image = UIImage.FromBundle("InfoIcon.png");
                item.SelectedImage = UIImage.FromBundle("InfoIcon.png");
                item.Image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.SelectedImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.ImageInsets = new UIEdgeInsets(5, 0, -5, 0);
            }
        }

        public override void UpdateLayout(UITabBarController controller)
        {
            UITabBar myTabBar = controller.TabBar;
            base.UpdateLayout(controller);
            if (myTabBar.Items != null && myTabBar.Items.Count() == 4)
            {
                UITabBarItem item = myTabBar.Items[0];

                item.Image = UIImage.FromBundle("HomeIcon.png");
                item.SelectedImage = UIImage.FromBundle("HomeIcon.png");
                item.Image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.SelectedImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.ImageInsets = new UIEdgeInsets(5, 0, -5, 0);

                item = myTabBar.Items[1];

                item.Image = UIImage.FromBundle("NewsIcon.png");
                item.SelectedImage = UIImage.FromBundle("NewsIcon.png");
                item.Image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.SelectedImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.ImageInsets = new UIEdgeInsets(5, 0, -5, 0);

                item = myTabBar.Items[2];

                item.Image = UIImage.FromBundle("EventsIcon.png");
                item.SelectedImage = UIImage.FromBundle("EventsIcon.png");
                item.Image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.SelectedImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.ImageInsets = new UIEdgeInsets(5, 0, -5, 0);


                item = myTabBar.Items[3];

                item.Image = UIImage.FromBundle("InfoIcon.png");
                item.SelectedImage = UIImage.FromBundle("InfoIcon.png");
                item.Image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.SelectedImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                item.ImageInsets = new UIEdgeInsets(5, 0, -5, 0);
            }
        }
    }
}