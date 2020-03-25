using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Grace2020.Controls
{
    public class GradientGrid : Grid
    {
        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create(nameof(StartColor), typeof(Color), typeof(GradientGrid));
        public Xamarin.Forms.Color StartColor
        {
            get => (Color)GetValue(StartColorProperty);
            set => SetValue(StartColorProperty, value);
        }

        public static readonly BindableProperty MiddleColorProperty =
            BindableProperty.Create(nameof(MiddleColor), typeof(Color), typeof(GradientGrid));
        public Xamarin.Forms.Color MiddleColor
        {
            get => (Color)GetValue(MiddleColorProperty);
            set => SetValue(MiddleColorProperty, value);
        }

        public static readonly BindableProperty EndColorProperty =
            BindableProperty.Create(nameof(EndColor), typeof(Color), typeof(GradientGrid));
        public Xamarin.Forms.Color EndColor
        {
            get => (Color)GetValue(EndColorProperty);
            set => SetValue(EndColorProperty, value);
        }

        //public GradientGrid()
        //{
        //    PropertyChanged += OnColorChanged;
        //}

        //private void OnColorChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if(e.PropertyName == nameof(StartColor))
        //    {
        //        foreach(var child in Children)
        //        {
        //            if(child is CustomFrame frame)
        //            {
        //                frame.ForceLayout();
        //            }
        //        }
        //    }
        //}
    }
}
