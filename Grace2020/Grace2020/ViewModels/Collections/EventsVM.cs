using GalaSoft.MvvmLight.Command;
using Grace2020.Models.Tables;
using Grace2020.Services;
using Grace2020.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.ViewModels.Collections
{
    public class EventsVM : AbstractCollectionVM
    {
        private ObservableCollection<Event> _events;
        public ObservableCollection<Event> Events
        {
            get { return _events; }
            set { Set(() => Events, ref _events, value); }
        }

        public event EventHandler OpenWeblinkUnsuccessful;

        public RelayCommand<string> OpenWebsite
        {
            get 
            { 
                return new RelayCommand<string>(async (link) =>
                {
                    var success = await WebService.OpenUri(link);
                    if (!success)
                    {
                        OpenWeblinkUnsuccessful?.Invoke(this, new EventArgs());
                    }
                }); 
            }
        }

        public EventsVM()
        {

        }

        protected override async Task LoadItems()
        {
            var webService = new WebService();
            await webService.GetEventsAsync();
            await webService.GetEventsImagesLookupAsync();
            Events = await LoadModelsAsync<Event>(getChildren:true, recursive:true);
        }
    }
}
