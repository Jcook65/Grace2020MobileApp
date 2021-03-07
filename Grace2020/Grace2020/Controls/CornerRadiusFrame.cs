using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Grace2020.Controls
{
    public class CornerRadiusFrame : Frame
    {
        public static new readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(CornerRadiusFrame));
        public new CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public CornerRadiusFrame()
        {
            base.CornerRadius = 0;
        }
    }
}
