using Grace2020.Models.Tables;
using Grace2020.Navigation;
using Grace2020.ViewModels.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Grace2020.Views.Collections
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopicsListVW : ContentPage, INotifyPropertyChanged
    {
        private PrayerHomeVM _vm;

        private ObservableCollection<ModulesLookup> _items;
        public ObservableCollection<ModulesLookup> Items
        {
            get { return _items; }
            set 
            { 
                if(value != _items)
                {
                    _items = value;
                    OnPropertyChanged(nameof(Items));
                } 
            }
        }

        public TopicsListVW()
        {
            InitializeComponent();

            topicsList.ItemTapped += (sender, e) =>
            {
                if(_vm != null)
                {
                    _vm.SelectedModulesLookup = e.Item as ModulesLookup;
                }
                NavigationService.GoTo("..");
            };
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            
            if (BindingContext is PrayerHomeVM vm)
            {
                _vm = vm;

                Items = _vm?.Topics ?? new ObservableCollection<ModulesLookup>();
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                Items = new ObservableCollection<ModulesLookup>(_vm?.Topics.Where(i => i.Topic.Title.Contains(e.NewTextValue)));
            }
            else
            {
                Items = _vm?.Topics ?? new ObservableCollection<ModulesLookup>();
            }
        }
    }
}