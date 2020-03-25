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

[assembly:ExportRenderer(typeof(GradientGrid), typeof(GradientGridRenderer_iOS))]
namespace Grace2020.iOS.Renderers
{
    public class GradientGridRenderer_iOS : ViewRenderer<Grid, UIView>
    {
        private Xamarin.Forms.Color _startColor;
        private Xamarin.Forms.Color _middleColor;
        private Xamarin.Forms.Color _endColor;

        protected override void OnElementChanged(ElementChangedEventArgs<Grid> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && e.NewElement is GradientGrid grid)
            {
                _startColor = grid.StartColor;
                _middleColor = grid.MiddleColor;
                _endColor = grid.EndColor;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Element is GradientGrid grid && 
                (e.PropertyName == nameof(grid.StartColor) ||
                 e.PropertyName == nameof(grid.MiddleColor) ||
                 e.PropertyName == nameof(grid.EndColor)))
            {
                _startColor = grid.StartColor;
                _middleColor = grid.MiddleColor;
                _endColor = grid.EndColor;
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            CGGradient gradient = null;
            using(CGColorSpace rgb = CGColorSpace.CreateDeviceRGB())
            {
                gradient = new CGGradient(rgb, new CGColor[]
                {
                    _startColor.ToCGColor(),
                    _middleColor.ToCGColor(),
                    _endColor.ToCGColor()
                });
            }
            var start = new CGPoint(0,0);
            var end = new CGPoint(rect.Width, rect.Height);
            for (var i = 0; i < Subviews.Length; i++)
            {
                var child = Subviews[i];
                if(child is FrameRenderer_iOS gFrame)
                {
                    gFrame.Gradient = gradient;
                    gFrame.Start = start;
                    gFrame.End = end;
                }
                
            }
        }
    }
}