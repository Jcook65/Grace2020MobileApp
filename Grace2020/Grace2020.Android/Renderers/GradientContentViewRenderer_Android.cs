using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Grace2020.Controls;
using Grace2020.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientContentView), typeof(GradientContentViewRenderer_Android))]
namespace Grace2020.Droid.Renderers
{
    public class GradientContentViewRenderer_Android : ViewRenderer
    {
        private Xamarin.Forms.Color _startColor;
        private Xamarin.Forms.Color _middleColor;
        private Xamarin.Forms.Color _endColor;

        public GradientContentViewRenderer_Android(Context context) 
            : base(context)
        {

        }

        protected override void DispatchDraw(Canvas canvas)
        {
            var gradient = new Android.Graphics.LinearGradient(
                0, 0, Width, Height,  
                new int[] { _startColor.ToAndroid(), _middleColor.ToAndroid(), _endColor.ToAndroid()},
                null,
                Android.Graphics.Shader.TileMode.Mirror);
            var paint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);
            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if(e.OldElement == null)
            {
                try
                {
                    if(e.NewElement is GradientContentView view)
                    {
                        _startColor = view.StartColor;
                        _middleColor = view.MiddleColor;
                        _endColor = view.EndColor;
                    }
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    throw ex;
                }
            }
        }
    }
}