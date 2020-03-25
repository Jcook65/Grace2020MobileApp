﻿using Grace2020.Models.Tables;
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

        public NewsDetailVM(News news)
        {
            _news = news;
        }
    }
}
