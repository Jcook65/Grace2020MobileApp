using GalaSoft.MvvmLight;
using Grace2020.Utils;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.ViewModels.Abstractions
{
    public abstract class VMBase : ViewModelBase
    {
        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(() => IsBusy, ref _isBusy, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { Set(() => Title, ref _title, value); }
        }

        private int _appTheme;
        public int AppTheme
        {
            get { return _appTheme; }
            set { Set(() => AppTheme, ref _appTheme, value); }
        }

        protected async Task<ObservableCollection<ModelType>> LoadModelsAsync<ModelType>(string sql, 
            bool getChildren, bool recursive, params object[] args) where ModelType : new()
        {
            try
            {
                List<ModelType> itemList;
                using(var db = new DbUtil())
                {
                    itemList = await db.AsyncConnection.QueryAsync<ModelType>(sql, args);
                    if (getChildren) { await GetModelsWithChildren(db.AsyncConnection, itemList, recursive); }
                }

                var models = new ObservableCollection<ModelType>(itemList);
                return models;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        protected async Task<ObservableCollection<ModelType>> LoadModelsAsync<ModelType>
            (Expression<Func<ModelType, bool>> whereClause = null,
              Expression<Func<ModelType, object>> orderBy = null,
              Expression<Func<ModelType, object>> orderByDesc = null,
              int? recordLimit = null,
              bool getChildren = false,
              bool recursive = false) where ModelType : new()
        {
            try
            {
                using(var db = new DbUtil())
                {
                    var items = db.AsyncConnection.Table<ModelType>();
                    if(whereClause != null)
                    {
                        items = items.Where(whereClause);
                    }
                    if(orderBy != null)
                    {
                        items = items.OrderBy(orderBy);
                    }
                    if(orderByDesc != null)
                    {
                        items = items.OrderByDescending(orderByDesc);
                    }
                    if(recordLimit != null)
                    {
                        items = items.Take(recordLimit.Value);
                    }
                    var itemList = await items.ToListAsync();
                    if(getChildren) await GetModelsWithChildren(db.AsyncConnection, itemList, recursive);
                    return new ObservableCollection<ModelType>(itemList);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        private async Task GetModelsWithChildren<ModelType>(SQLiteAsyncConnection con, List<ModelType> items, bool recursive = false) where ModelType : new()
        {
            foreach (ModelType item in items)
            {
                await con.GetChildrenAsync(item, recursive);
            }
        }
    }
}
