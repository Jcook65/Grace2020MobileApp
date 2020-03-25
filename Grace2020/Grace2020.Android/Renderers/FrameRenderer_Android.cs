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
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(FrameRenderer_Android))]
namespace Grace2020.Droid.Renderers
{
    public class FrameRenderer_Android : Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        public LinearGradient Gradient { get; set; }

        public FrameRenderer_Android(Context context)
            : base(context) { }

        protected override void OnDraw(Canvas canvas)
        {
            if(Control != null && Element is CustomFrame frame)
            {
                var paint = new Android.Graphics.Paint()
                {
                    Dither = true,
                };
                if (Gradient!= null)
                {
                    paint.SetShader(Gradient);
                }
                var path = RoundedRect(0, 0, Width, Height, Context.ToPixels(frame.CornerRadius.TopRight), Context.ToPixels(frame.CornerRadius.TopLeft), true);
                canvas.DrawPath(path, paint);
            }
            base.OnDraw(canvas);
        }

        static public Path RoundedRect(float left, float top, float width, float height, float rx, float ry, bool conformToOriginalPost)
        {
            Path path = new Path();
            if (rx < 0) rx = 0;
            if (ry < 0) ry = 0;
            float right = width + left;
            if (rx > width / 2) rx = width / 2;
            if (ry > height / 2) ry = height / 2;
            float widthMinusCorners = (width - (2 * rx));
            float heightMinusCorners = (height - (2 * ry));

            path.MoveTo(right, top + ry);
            path.RQuadTo(0, -ry, -rx, -ry);//top-right corner
            path.RLineTo(-widthMinusCorners, 0);
            path.RQuadTo(-rx, 0, -rx, ry); //top-left corner
            path.RLineTo(0, heightMinusCorners);

            if (conformToOriginalPost)
            {
                path.RLineTo(0, ry);
                path.RLineTo(width, 0);
                path.RLineTo(0, -ry);
            }
            else
            {
                path.RQuadTo(0, ry, rx, ry);//bottom-left corner
                path.RLineTo(widthMinusCorners, 0);
                path.RQuadTo(rx, 0, rx, -ry); //bottom-right corner
            }

            path.RLineTo(0, -heightMinusCorners);

            path.Close();//Given close, last lineto can be removed.

            return path;
        }
    }
}