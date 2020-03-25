using Grace2020.DependencyInjection;
using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.Utils
{
    public class DbUtil : IDisposable
    {
        private ISQLite _db;
        public SQLiteAsyncConnection AsyncConnection { get; }

        public DbUtil()
        {
            _db = Xamarin.Forms.DependencyService.Get<ISQLite>();
            AsyncConnection = _db.ConnectAsync();
        }

        private static async Task<bool> DbExists(string filePath)
        {
            var db = await FileSystem.Current.GetFileFromPathAsync(filePath);
            if (db != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task InitDbAsync(string[] tablesNameSpaces)
        {

            var db = await FileSystem.Current.GetFileFromPathAsync(Path.Combine(
                            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                            Xamarin.Forms.DependencyService.Get<ISQLite>().DatabaseName));
            db?.DeleteAsync().Wait();

            using (var dbUtil = new DbUtil())
            {
                foreach (var name in tablesNameSpaces)
                {
                    var typeInfos = typeof(DbUtil)
                        .Assembly
                        .DefinedTypes
                        .Where(t => string.Equals(t.Namespace, name, StringComparison.Ordinal))
                        .Where(t => !t.IsInterface && !t.IsAbstract && !t.IsEnum && t.IsPublic)
                        .ToArray();
                    var types = typeInfos.Select(t => t.AsType()).ToArray();

                    await dbUtil.AsyncConnection.CreateTablesAsync(types: types);
                }
            }
        }

        public void Dispose()
        {
            _db?.Dispose();
            _db = null;
            GC.SuppressFinalize(this);
        }
    }
}
