using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Grace2020.DependencyInjection;

[assembly: Xamarin.Forms.Dependency(typeof(Grace2020.Droid.DependencyInjection.SQLite_Droid))]
namespace Grace2020.Droid.DependencyInjection
{
    public class SQLite_Droid : AbstractSQLite
    {
        public override string DatabaseName => "Grace2020.sqlite";
        protected override string GetDatabasePath() =>
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); //TODO: add platform specific File Path Systems
    }
}