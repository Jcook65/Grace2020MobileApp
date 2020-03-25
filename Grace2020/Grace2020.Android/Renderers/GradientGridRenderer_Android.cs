using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Grace2020.Controls;
using Grace2020.Droid.Renderers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(GradientGrid), typeof(GradientGridRenderer_Android))]
namespace Grace2020.Droid.Renderers
{
    public class GradientGridRenderer_Android : ViewRenderer
    {
        private Xamarin.Forms.Color _startColor;
        private Xamarin.Forms.Color _middleColor;
        private Xamarin.Forms.Color _endColor;

        LinearGradient _gradient;

        public GradientGridRenderer_Android(Context context)
            : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if(e.NewElement != null && e.NewElement is GradientGrid grid)
            {
                _startColor = grid.StartColor;
                _middleColor = grid.MiddleColor;
                _endColor = grid.EndColor;
            }
        }

        protected override void DispatchDraw(Canvas canvas)
        {
            base.DispatchDraw(canvas);
            _gradient = new LinearGradient(
                    0, 0, Width, Height,
                    new int[]
                    {
                        _startColor.ToAndroid(),
                        _middleColor.ToAndroid(),
                        //_endColor.ToAndroid(),
                    },
                    null,
                    Shader.TileMode.Mirror);
            for (var i = 0; i < ChildCount; i++)
            {
                var child = GetChildAt(i);
                if(child is FrameRenderer_Android gFrame)
                {
                    gFrame.Gradient = _gradient;
                    gFrame.Invalidate();
                }
            }
        }
    }
}