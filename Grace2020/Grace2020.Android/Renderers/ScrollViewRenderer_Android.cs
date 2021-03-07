

using Android.Content;
using Grace2020.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ScrollView), typeof(ScrollViewRenderer_Android))]
namespace Grace2020.Droid.Renderers
{
    public class ScrollViewRenderer_Android : ScrollViewRenderer
    {
        public ScrollViewRenderer_Android(Context context) 
            : base(context)
        {

        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if(e.NewElement != null)
                this.OverScrollMode = Android.Views.OverScrollMode.Never;
        }
    }
}