using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Grace2020.Controls
{
    public class GradientFrame : Frame
    {
        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create(nameof(StartColor), typeof(Color), typeof(GradientFrame));
        public Xamarin.Forms.Color StartColor
        {
            get => (Color)GetValue(StartColorProperty);
            set => SetValue(StartColorProperty, value);
        }

        public static readonly BindableProperty MiddleColorProperty =
            BindableProperty.Create(nameof(MiddleColor), typeof(Color), typeof(GradientFrame));
        public Xamarin.Forms.Color MiddleColor
        {
            get => (Color)GetValue(MiddleColorProperty);
            set => SetValue(MiddleColorProperty, value);
        }

        public static readonly BindableProperty EndColorProperty =
            BindableProperty.Create(nameof(EndColor), typeof(Color), typeof(GradientFrame));
        public Xamarin.Forms.Color EndColor
        {
            get => (Color)GetValue(EndColorProperty);
            set => SetValue(EndColorProperty, value);
        }

        public static new readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(GradientFrame));
        public new CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
