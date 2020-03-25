using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Grace2020.DependencyInjection;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Grace2020.iOS.DependencyInjection.SQLite_iOS))]
namespace Grace2020.iOS.DependencyInjection
{
    public class SQLite_iOS : AbstractSQLite
    {
        public override string DatabaseName => "Grace2020.sqlite";
        protected override string GetDatabasePath() => Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    }
}