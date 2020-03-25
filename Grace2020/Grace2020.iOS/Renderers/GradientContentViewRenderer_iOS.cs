using System;
using System.Collections.Generic;
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

[assembly: ExportRenderer(typeof(GradientContentView), typeof(GradientContentViewRenderer_iOS))]
namespace Grace2020.iOS.Renderers
{
    public class GradientContentViewRenderer_iOS : ViewRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            GradientContentView contentView = (GradientContentView)Element;

            CGColor startColor = contentView.StartColor.ToCGColor();
            CGColor middleColor = contentView.MiddleColor.ToCGColor();
            CGColor endColor = contentView.EndColor.ToCGColor();

            var gradientLayer = new CAGradientLayer()
            {
                StartPoint = new CGPoint(0, 0.5),
                EndPoint = new CGPoint(1, 0.5)
            };
            gradientLayer.Frame = rect;
            gradientLayer.Colors = new CGColor[] { startColor, middleColor, endColor};
            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}