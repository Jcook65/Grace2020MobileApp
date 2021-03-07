using GalaSoft.MvvmLight.Command;
using Grace2020.Models.Tables;
using Grace2020.Navigation;
using Grace2020.Resources;
using Grace2020.Services;
using Grace2020.Utils;
using Grace2020.ViewModels.Abstractions;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.ViewModels.Collections
{
    public class ModulesVM : AbstractCollectionVM
    {
        public string ENGreeting => GetProperGreetingForTimeOfDay("en");
        public string JAGreeting => GetProperGreetingForTimeOfDay("ja");
        public string Date => DateTime.Now.Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern);
        public bool JumpBackInEnabled { get; private set; }

        private ObservableCollection<Module> _modules;
        public ObservableCollection<Module> Modules
        {
            get { return _modules; }
            set { Set(() => Modules, ref _modules, value); }
        }

        public RelayCommand<Module> NavigateToPrayerHome { get; private set; }
        public RelayCommand JumpBackIn { get; set; }

        public ModulesVM() 
        {
            NavigateToPrayerHome = new RelayCommand<Module>(async (module) =>
            {
                await CurrentUserUtil.UpdateCurrentUserConfigAsync(module?.ModuleId);
                var vm = new PrayerHomeVM(module);
                NavigationService.GoTo("///Home", vm);
                await vm.Load();
            });

            JumpBackIn = new RelayCommand(async () =>
            {
                var config = await CurrentUserUtil.GetCurrentUserConfigurationAsync();
                if (config?.CurrentTopicId != null)
                {
                    using (var db = new DbUtil())
                    {
                        var moduleLookup = await db.AsyncConnection.FindAsync<ModulesLookup>(i => i.ModuleLookupId == config.CurrentTopicId);
                        var vm = new PrayerHomeVM(moduleLookup);
                        NavigationService.GoTo("///Home", vm);
                        await vm.Load();
                    }
                }
            });
        }

        private string GetProperGreetingForTimeOfDay(string lang)
        {
            if(DateTime.Now.TimeOfDay.Hours > 3 && DateTime.Now.TimeOfDay.Hours <  12)
            {
                return lang == "en" ? "Good Morning" : "おはようございます";
            }
            else if(DateTime.Now.TimeOfDay.Hours >= 12 && DateTime.Now.TimeOfDay.Hours < 17)
            {
                return lang == "en" ? "Good Afternoon" : "こんにちは";
            }
            else
            {
                return lang == "en" ? "Good Evening" : "こんばんは";
            }
        }

        protected override async Task LoadItems()
        {
            var webService = new WebService();
            await webService.GetModulesAsync();
            var config = await CurrentUserUtil.GetCurrentUserConfigurationAsync();
            if(config?.CurrentTopicId != null)
            {
                JumpBackInEnabled = true;
                RaisePropertyChanged(nameof(JumpBackInEnabled));
            }
            Modules = await LoadModelsAsync<Module>();
        }
    }
}
