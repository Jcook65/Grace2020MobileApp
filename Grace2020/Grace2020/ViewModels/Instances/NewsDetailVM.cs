using GalaSoft.MvvmLight.Command;
using Grace2020.Models.Tables;
using Grace2020.Navigation;
using Grace2020.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.ViewModels.Instances
{
    public class NewsDetailVM : VMBase
    {
        private News _news;
        public News News
        {
            get { return _news; }
            set { Set(() => News, ref _news, value); }
        }

        public RelayCommand<SelectableItemVM> EnlargeImage { get; private set; }

        public NewsDetailVM(News news)
        {
            _news = news;
        }
    }
}
