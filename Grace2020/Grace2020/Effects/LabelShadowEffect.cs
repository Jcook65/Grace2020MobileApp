using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Grace2020.Effects
{
    public class LabelShadowEffect : RoutingEffect
	{
		public Color Color { get; set; }
		public float Radius { get; set; }
		public float DistanceX { get; set; }
		public float DistanceY { get; set; }
		public LabelShadowEffect() : base("Grace.ShadowEffect")
		{
		}
	}
}
