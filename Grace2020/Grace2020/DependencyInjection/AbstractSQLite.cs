using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Grace2020.DependencyInjection
{
    public abstract class AbstractSQLite : ISQLite
    {
        private object dbLock = new object();
        private object conLock = new object();
        protected static SQLiteConnection _connection;
        protected static SQLiteAsyncConnection _asyncConnection;
        public abstract string DatabaseName { get; }


        protected abstract string GetDatabasePath();

        public SQLiteConnection Connect()
        {
            if(_connection != null)
            {
                return _connection;
            }

            lock (conLock)
            {
                if(_connection == null)
                {
                    if(_asyncConnection == null)
                    {
                        ConnectAsync();
                    }
                    _connection = _asyncConnection.GetConnection();
                }
                return _connection;
            }
        }

        public SQLiteAsyncConnection ConnectAsync()
        {
            if(_asyncConnection != null)
            {
                return _asyncConnection;
            }
            lock (dbLock)
            {
                if(_asyncConnection != null)
                {
                    return _asyncConnection;
                }
                string dbPath = GetDatabasePath();
                string path = Path.Combine(dbPath, DatabaseName);
                _asyncConnection = new SQLiteAsyncConnection(path, false);
                Task.Run(async () => await _asyncConnection.ExecuteAsync("PRAGMA encoding='UTF-8'"));
                return _asyncConnection;
            }
        }

        public void Dispose()
        {
            Debug.WriteLine("SQLite Connection Dispose was called");
            return;
        }
    }
}
