using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Grace2020.Controls;
using Grace2020.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientFrame), typeof(GradientFrameRenderer_Android))]
namespace Grace2020.Droid.Renderers
{
    public class GradientFrameRenderer_Android : Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        public GradientFrameRenderer_Android(Context context)
            : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && Control != null && e.NewElement is GradientFrame frame)
            {
                var gradient = new GradientDrawable(GradientDrawable.Orientation.TlBr, new[]
                {
                    frame.StartColor.ToAndroid().ToArgb(),
                    frame.MiddleColor.ToAndroid().ToArgb(),
                    frame.EndColor.ToAndroid().ToArgb()
                });
                ViewCompat.SetBackground(this, gradient);
                UpdateCornerRadius();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(GradientFrame.CornerRadius) || e.PropertyName == nameof(GradientFrame))
            {
                UpdateCornerRadius();
            }
        }

        private void UpdateCornerRadius()
        {
            if (Control.Background is GradientDrawable backgroundGradient)
            {
                var cornerRadius = (Element as GradientFrame)?.CornerRadius;
                if (!cornerRadius.HasValue)
                {
                    return;
                }
                var topLeftCorner = Context.ToPixels(cornerRadius.Value.TopLeft);
                var topRightCorner = Context.ToPixels(cornerRadius.Value.TopRight);
                var bottomLeftCorner = Context.ToPixels(cornerRadius.Value.BottomLeft);
                var bottomRightCorner = Context.ToPixels(cornerRadius.Value.BottomRight);

                var cornerRadii = new[]
                {
                    topLeftCorner,
                    topLeftCorner,

                    topRightCorner,
                    topRightCorner,

                    bottomRightCorner,
                    bottomRightCorner,

                    bottomLeftCorner,
                    bottomLeftCorner,
                };
                backgroundGradient.SetCornerRadii(cornerRadii);
            }
        }
    }
}