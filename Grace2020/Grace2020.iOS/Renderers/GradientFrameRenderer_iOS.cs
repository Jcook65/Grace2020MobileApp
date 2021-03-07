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
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(GradientFrame), typeof(GradientFrameRenderer_iOS))]
namespace Grace2020.iOS.Renderers
{
    public class GradientFrameRenderer_iOS : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            if(Element is GradientFrame frame)
            {
                CGColor startColor = frame.StartColor.ToCGColor();
                CGColor middleColor = frame.MiddleColor.ToCGColor();
                CGColor endColor = frame.EndColor.ToCGColor();

                var gradientLayer = new CAGradientLayer()
                {
                    StartPoint = new CGPoint(0, 0),
                    EndPoint = new CGPoint(1, 1),
                    Frame = rect,
                    Colors = new CGColor[] { startColor, middleColor, endColor }
                };
                NativeView.Layer.BackgroundColor = UIColor.Clear.CGColor;
                NativeView.Layer.InsertSublayer(gradientLayer, 0);
            }

            base.Draw(rect);
        }

        //public override void LayoutSubviews()
        //{
        //    base.LayoutSubviews();
        //    if (Element is GradientFrame frame)
        //    {
        //        UpdateCornerRadius();
        //    }
        //}
        //
        //protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    base.OnElementPropertyChanged(sender, e);
        //
        //    if (e.PropertyName == nameof(GradientFrame.CornerRadius) ||
        //        e.PropertyName == nameof(GradientFrame))
        //    {
        //        UpdateCornerRadius();
        //    }
        //}
        //
        //private double RetrieveCommonCornerRadius(CornerRadius cornerRadius)
        //{
        //    var commonCornerRadius = cornerRadius.TopLeft;
        //    if (commonCornerRadius <= 0)
        //    {
        //        commonCornerRadius = cornerRadius.TopRight;
        //        if (commonCornerRadius <= 0)
        //        {
        //            commonCornerRadius = cornerRadius.BottomLeft;
        //            if (commonCornerRadius <= 0)
        //            {
        //                commonCornerRadius = cornerRadius.BottomRight;
        //            }
        //        }
        //    }
        //
        //    return commonCornerRadius;
        //}
        //
        //private UIRectCorner RetrieveRoundedCorners(CornerRadius cornerRadius)
        //{
        //    var roundedCorners = default(UIRectCorner);
        //
        //    if (cornerRadius.TopLeft > 0)
        //    {
        //        roundedCorners |= UIRectCorner.TopLeft;
        //    }
        //
        //    if (cornerRadius.TopRight > 0)
        //    {
        //        roundedCorners |= UIRectCorner.TopRight;
        //    }
        //
        //    if (cornerRadius.BottomLeft > 0)
        //    {
        //        roundedCorners |= UIRectCorner.BottomLeft;
        //    }
        //
        //    if (cornerRadius.BottomRight > 0)
        //    {
        //        roundedCorners |= UIRectCorner.BottomRight;
        //    }
        //
        //    return roundedCorners;
        //}
        //
        //private void UpdateCornerRadius()
        //{
        //    var cornerRadius = (Element as GradientFrame)?.CornerRadius;
        //    if (!cornerRadius.HasValue)
        //    {
        //        return;
        //    }
        //
        //    var roundedCornerRadius = RetrieveCommonCornerRadius(cornerRadius.Value);
        //    if (roundedCornerRadius <= 0)
        //    {
        //        return;
        //    }
        //
        //    var roundedCorners = RetrieveRoundedCorners(cornerRadius.Value);
        //
        //    var path = UIBezierPath.FromRoundedRect(Bounds, roundedCorners, new CGSize(roundedCornerRadius, roundedCornerRadius));
        //    var mask = new CAShapeLayer { Path = path.CGPath };
        //    NativeView.Layer.Mask = mask;
        //}
    }
}