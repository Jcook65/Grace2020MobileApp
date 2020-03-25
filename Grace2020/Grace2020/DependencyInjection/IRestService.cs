using Grace2020.Models;
using Grace2020.Models.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.DependencyInjection
{
    public interface IRestService
    {
        Task RefreshDataAsync<TModel>(string url, string jsonPackage) where TModel : IModel, new();
    }
}
