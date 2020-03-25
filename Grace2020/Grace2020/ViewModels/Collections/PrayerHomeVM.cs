using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Grace2020.DependencyInjection;
using Grace2020.Models.Tables;
using Grace2020.Navigation;
using Grace2020.Services;
using Grace2020.Utils;
using Grace2020.ViewModels.Abstractions;
using Grace2020.ViewModels.Instances;
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
        private ModulesLookup _selectedModulesLookup;
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

        private Module _selectedModule;
        public Module SelectedModule
        {
            get { return _selectedModule; }
            set 
            {
                var previous = SelectedModule;
                Set(() => SelectedModule, ref _selectedModule, value);
                if(SelectedModule != previous)
                {
                    ModuleSelected?.Invoke(this, new EventArgs());
                }
            }
        }

        private ObservableCollection<ModulesLookup> _prayers;
        public ObservableCollection<ModulesLookup> Prayers
        {
            get { return _prayers; }
            set { Set(() => Prayers, ref _prayers, value); }
        }

        private ObservableCollection<Module> _modules;
        public ObservableCollection<Module> Modules
        {
            get { return _modules; }
            set { Set(() => Modules, ref _modules, value); }
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

        public event EventHandler ModulesLoaded;
        public event EventHandler<ModulesLookup> ModulesLookupSelected;
        public event EventHandler ModuleSelected;
        public event EventHandler PrayersLoaded;

        public PrayerHomeVM()
        {
            SetSearchCommand();
            ModulesLoaded += OnModulesLoaded;
            ModuleSelected += OnModuleSelected;
            PrayersLoaded += OnPrayersLoaded;
        }

        public PrayerHomeVM(ModulesLookup prayer)
        {
            SetSearchCommand();
            SelectedModulesLookup = prayer;
            ModulesLoaded += OnModulesLoaded;
            ModuleSelected += OnModuleSelected;
            PrayersLoaded += OnPrayersLoaded;
        }

        protected override async Task LoadItems()
        {
            var webService = new WebService();
            await webService.GetPrayersAsync();
            await webService.GetModulesLookupAsync();
            await webService.GetModulesAsync();

            Modules = await LoadModelsAsync<Module>();
            ModulesLoaded?.Invoke(this, new EventArgs());
            await LoadPrayers();
        }

        private async Task LoadPrayers()
        {
            Prayers = await LoadModelsAsync(BuildWhereClause(), getChildren: true, recursive: true);
            PrayersLoaded?.Invoke(this, new EventArgs());
        }

        private void SetSearchCommand()
        {
            Search = new RelayCommand(() =>
            {
                if (!string.IsNullOrWhiteSpace(SearchCriteria?.Trim()))
                {
                    Searching = true;
                    SearchCriteria = SearchCriteria.Trim();
                    var prayer = Prayers.ToList().Find(i => i.Prayer.TitleText.ToUpper().Contains(SearchCriteria.ToUpper()));
                    SelectedModulesLookup = prayer ?? SelectedModulesLookup;
                    Searching = false;
                    SearchCriteria = null;
                }
            });
        }

        private Expression<Func<ModulesLookup, bool>> BuildWhereClause()
        {
            Expression<Func<ModulesLookup, bool>> expression = i => i.ModuleId == 1;
            if(SelectedModule != null)
            {
                expression = i => i.ModuleId == SelectedModule.ModuleId;
            }

            return expression;
        }

        private void OnModulesLoaded(object sender, EventArgs e)
        {
            SelectedModule = Modules?.FirstOrDefault();
        }

        private void OnModuleSelected(object sender, EventArgs e)
        {
            Task.Run(async () => await LoadPrayers());
        }

        private void OnPrayersLoaded(object sender, EventArgs e)
        {
            SelectedModulesLookup = Prayers?.Where(i => i.Date.HasValue && i.Date.Value.Date == DateTime.Now.Date).FirstOrDefault() ?? Prayers?.FirstOrDefault();
        }
    }
}
