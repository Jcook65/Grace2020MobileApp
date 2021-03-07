using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.DependencyInjection
{
    interface IWebService
    {
        Task GetPrayersAsync();
        Task GetNewsAsync();
        Task GetNewsAssetLookupAsync();
    }
}
