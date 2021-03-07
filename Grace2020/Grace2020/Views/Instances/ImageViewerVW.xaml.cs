using Grace2020.ViewModels.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Instances
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageViewerVW : ContentPage
    {
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;

        public ImageViewerVW()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        private void OnImagePinched(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the compone nts for the center point of the translate transform.
                startScale = imageView.Scale;
                imageView.AnchorX = 0;
                imageView.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = imageView.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (imageView.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = imageView.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (imageView.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * imageView.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * imageView.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.
                imageView.TranslationX = targetX.Clamp(-imageView.Width * (currentScale - 1), 0);
                imageView.TranslationY = targetY.Clamp(-imageView.Height * (currentScale - 1), 0);

                // Apply scale factor.
                imageView.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                xOffset = imageView.TranslationX;
                yOffset = imageView.TranslationY;
            }
        }
    }
}