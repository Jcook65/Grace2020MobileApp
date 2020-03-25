using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Grace2020.Resources
{
    public static class Constants
    {
        public static string BaseAddress = "https://grace2020.org";
        public static string GRACE2020ServiceUrl = BaseAddress + "/app/mobile/{0}";
        public static string GRACE2020NewsImageUrl = BaseAddress + "/app/images/NewsImages/{0}";
        public static string GRACE2020EventImageUrl = BaseAddress + "/app/images/EventImages/{0}";
        public static string GRACE2020RegionImageUrl = BaseAddress + "/app/images/RegionImages/{0}";
    }
}
