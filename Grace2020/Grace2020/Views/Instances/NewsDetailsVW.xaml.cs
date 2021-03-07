using Grace2020.Models.Tables;
using Grace2020.Navigation;
using Grace2020.Utils;
using Grace2020.ViewModels.Instances;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Instances
{
    [QueryProperty("NewsId", "newsid")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsDetailsVW : ContentPage
    {
        private string _newsId;
        public string NewsId
        {
            get { return _newsId; }
            set { _newsId = value; NewsPropertyChanged?.Invoke(this, _newsId); }
        }

        public event EventHandler<string> NewsPropertyChanged;

        public NewsDetailsVW()
        {
            InitializeComponent();
            NewsPropertyChanged += OnNewsPropertyChanged;
        }

        private void OnNewsPropertyChanged(object sender, string e)
        {
            if(!string.IsNullOrWhiteSpace(e))
            {
                using (var db = new DbUtil())
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var news = await db.AsyncConnection.FindAsync<News>(i => i.NewsId == NewsId);
                        await db.AsyncConnection.GetChildrenAsync(news);
                        BindingContext = new NewsDetailVM(news);
                    });
                }
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(BindingContext is NewsDetailVM bindingContext)
            {
                var items = bindingContext.News?.Images;
                pictureCarousel.ItemsSource = items;
            }
        }
    }
}