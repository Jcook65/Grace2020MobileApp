using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Grace2020.DependencyInjection;
using Grace2020.Models.Tables;
using Grace2020.Navigation;
using Grace2020.Services;
using Grace2020.ViewModels.Abstractions;
using Grace2020.ViewModels.Instances;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.ViewModels.Collections
{
    public class NewsVM : AbstractCollectionVM
    {
        private ObservableCollection<News> _newsPosts;
        public ObservableCollection<News> NewsPosts
        {
            get { return _newsPosts; }
            set { Set(() => NewsPosts, ref _newsPosts, value); }
        }

        private News _selectedNews;
        public News SelectedNews
        {
            get { return _selectedNews; }
            set
            {
                Set(() => SelectedNews, ref _selectedNews, value);
                if(SelectedNews != null)
                {
                    ItemSelected?.Invoke(this, SelectedNews);
                }
            }
        }

        public event EventHandler<News> ItemSelected;
        public NewsVM()
        {
            ItemSelected += OnItemSelected;
        }

        protected override async Task LoadItems()
        {
            var webService = new WebService();
            await webService.GetNewsAsync();
            await webService.GetNewsAssetLookupAsync();
            NewsPosts = await LoadModelsAsync<News>(getChildren:true, recursive:true);
        }

        private void OnItemSelected(object sender, News e)
        {
            NavigationService.GoTo($"{ViewLocator.NewsDetailPage}?newsid={e.NewsId}");
        }
    }
}
