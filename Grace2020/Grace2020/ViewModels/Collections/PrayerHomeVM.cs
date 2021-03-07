using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Grace2020.DependencyInjection;
using Grace2020.Models.Tables;
using Grace2020.Navigation;
using Grace2020.Services;
using Grace2020.Utils;
using Grace2020.ViewModels.Abstractions;
using Grace2020.ViewModels.Instances;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.ViewModels.Collections
{
    public class PrayerHomeVM : AbstractCollectionVM
    {
        private Module _selectedModule;

        private static ModulesLookup _selectedModulesLookup;
        public ModulesLookup SelectedModulesLookup
        {
            get { return _selectedModulesLookup; }
            set 
            {
                var previous = SelectedModulesLookup;
                Set(() => SelectedModulesLookup, ref _selectedModulesLookup, value);
                if(SelectedModulesLookup != previous)
                {
                    ModulesLookupSelected?.Invoke(this, SelectedModulesLookup); 
                }
            }
        }

        private ObservableCollection<ModulesLookup> _topics;
        public ObservableCollection<ModulesLookup> Topics
        {
            get { return _topics; }
            set { Set(() => Topics, ref _topics, value); }
        }

        private string _searchCriteria;
        public string SearchCriteria
        {
            get { return _searchCriteria; }
            set { Set(() => SearchCriteria, ref _searchCriteria, value); }
        }

        private bool _searching;
        public bool Searching
        {
            get { return _searching; }
            set { Set(() => Searching, ref _searching, value); }
        }

        public RelayCommand Search { get; private set; }

        public event EventHandler<ModulesLookup> ModulesLookupSelected;
        public event EventHandler TopicsLoaded;

        public PrayerHomeVM()
        {
            SetSearchCommand();
            TopicsLoaded += OnTopicsLoaded;
        }

        public PrayerHomeVM(Module module)
        {
            SetSearchCommand();
            _selectedModule = module;
            _selectedModulesLookup = null;
            TopicsLoaded += OnTopicsLoaded;
            ModulesLookupSelected += OnModuleLookupSelected;
        }

        public PrayerHomeVM(ModulesLookup moduleLookup)
        {
            SetSearchCommand();
            _selectedModulesLookup = moduleLookup;
            TopicsLoaded += OnTopicsLoaded;
            ModulesLookupSelected += OnModuleLookupSelected;
        }

        protected override async Task LoadItems()
        {
            var webService = new WebService();
            await webService.GetTopicsAsync();
            await webService.GetPrayersAsync();
            await webService.GetModulesLookupAsync();
            await LoadTopics();
        }

        private async Task LoadTopics()
        {
            Topics = await LoadModelsAsync(BuildWhereClause(), orderBy:i => i.Sequence, getChildren: true, recursive: true);
            TopicsLoaded?.Invoke(this, new EventArgs());
        }

        private void SetSearchCommand()
        {
            Search = new RelayCommand(() =>
            {
                if (!string.IsNullOrWhiteSpace(SearchCriteria?.Trim()))
                {
                    Searching = true;
                    SearchCriteria = SearchCriteria.Trim();
                    var topic = Topics.ToList().Find(i => i.Topic.Title.ToUpper().Contains(SearchCriteria.ToUpper()));
                    SelectedModulesLookup = topic ?? SelectedModulesLookup;
                    Searching = false;
                    SearchCriteria = null;
                }
            });
        }

        private Expression<Func<ModulesLookup, bool>> BuildWhereClause()
        {
            Expression<Func<ModulesLookup, bool>> expression = i => i.ModuleId == "Alphabetical";
            
            if(_selectedModulesLookup != null)
            {
                expression = i => i.ModuleId == _selectedModulesLookup.ModuleId;
            }
            else if(_selectedModule?.ModuleId != null)
            {
                expression = i => i.ModuleId == _selectedModule.ModuleId;
            }

            return expression;
        }

        private void OnTopicsLoaded(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                using (var db = new DbUtil())
                {
                    var config = await CurrentUserUtil.GetCurrentUserConfigurationAsync();
                    ModulesLookup moduleLookup = null;
                    if (config != null && config.CurrentTopicId != null && (Topics?.Any(i => i.ModuleLookupId == config.CurrentTopicId) ?? false))
                    {

                        moduleLookup = await db.AsyncConnection.FindAsync<ModulesLookup>(i => i.ModuleLookupId == config.CurrentTopicId);
                    }
                    else
                    {
                        moduleLookup = Topics?.Where(i => i.Date.HasValue && i.Date.Value.Date == DateTime.Now.Date).FirstOrDefault() ?? Topics?.FirstOrDefault();
                    }

                    if(moduleLookup != null) await db.AsyncConnection.GetChildrenAsync(moduleLookup, recursive:true);
                    SelectedModulesLookup = moduleLookup;
                }
            });
        }

        private void OnModuleLookupSelected(object sender, ModulesLookup e)
        {
            if(e != null)
            {
                Task.Run(async () => await CurrentUserUtil.UpdateCurrentUserConfigAsync(e.ModuleId, e.ModuleLookupId));
            }
        }
    }
}
