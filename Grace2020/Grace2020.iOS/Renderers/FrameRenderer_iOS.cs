using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Grace2020.Controls;
using Grace2020.iOS.Renderers;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(CustomFrame), typeof(FrameRenderer_iOS))]
namespace Grace2020.iOS.Renderers
{
    public class FrameRenderer_iOS : FrameRenderer
    {
        public CGGradient Gradient { get; set; }
        public CGPoint Start { get; set; }
        public CGPoint End { get; set; }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            using(CGContext context = UIGraphics.GetCurrentContext())
            {
                context.DrawLinearGradient(Gradient, Start, End, CGGradientDrawingOptions.DrawsBeforeStartLocation);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (Element is CustomFrame frame)
            {   
                UpdateCornerRadius();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(CustomFrame.CornerRadius) ||
                e.PropertyName == nameof(CustomFrame))
            {
                UpdateCornerRadius();
            }
        }

        private double RetrieveCommonCornerRadius(CornerRadius cornerRadius)
        {
            var commonCornerRadius = cornerRadius.TopLeft;
            if (commonCornerRadius <= 0)
            {
                commonCornerRadius = cornerRadius.TopRight;
                if (commonCornerRadius <= 0)
                {
                    commonCornerRadius = cornerRadius.BottomLeft;
                    if (commonCornerRadius <= 0)
                    {
                        commonCornerRadius = cornerRadius.BottomRight;
                    }
                }
            }

            return commonCornerRadius;
        }

        private UIRectCorner RetrieveRoundedCorners(CornerRadius cornerRadius)
        {
            var roundedCorners = default(UIRectCorner);

            if (cornerRadius.TopLeft > 0)
            {
                roundedCorners |= UIRectCorner.TopLeft;
            }

            if (cornerRadius.TopRight > 0)
            {
                roundedCorners |= UIRectCorner.TopRight;
            }

            if (cornerRadius.BottomLeft > 0)
            {
                roundedCorners |= UIRectCorner.BottomLeft;
            }

            if (cornerRadius.BottomRight > 0)
            {
                roundedCorners |= UIRectCorner.BottomRight;
            }

            return roundedCorners;
        }

        private void UpdateCornerRadius()
        {
            var cornerRadius = (Element as CustomFrame)?.CornerRadius;
            if (!cornerRadius.HasValue)
            {
                return;
            }

            var roundedCornerRadius = RetrieveCommonCornerRadius(cornerRadius.Value);
            if (roundedCornerRadius <= 0)
            {
                return;
            }

            var roundedCorners = RetrieveRoundedCorners(cornerRadius.Value);

            var path = UIBezierPath.FromRoundedRect(Bounds, roundedCorners, new CGSize(roundedCornerRadius, roundedCornerRadius));
            var mask = new CAShapeLayer { Path = path.CGPath };
            NativeView.Layer.Mask = mask;
        }
    }
}