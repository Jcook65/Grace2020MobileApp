using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Grace2020.Controls
{
    public class CustomFrame : Frame
    {
        public static new readonly BindableProperty CornerRadiusProperty = 
            BindableProperty.Create(nameof(CustomFrame), typeof(CornerRadius), typeof(CustomFrame));

        public new CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public CustomFrame()
        {
            base.CornerRadius = 0;
        }

    }
}
