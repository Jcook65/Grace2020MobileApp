using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.DependencyInjection
{
    public interface ISQLite : IDisposable
    {
        string DatabaseName { get; }
        SQLiteConnection Connect();
        SQLiteAsyncConnection ConnectAsync();
    }
}
