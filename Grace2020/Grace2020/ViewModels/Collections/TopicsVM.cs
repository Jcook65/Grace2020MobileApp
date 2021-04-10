using Grace2020.Models.Tables;
using Grace2020.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.ViewModels.Collections
{
    public class TopicsVM : AbstractCollectionVM
    {
        private ObservableCollection<ModulesLookup> _topics;
        public ObservableCollection<ModulesLookup> Topics
        {
            get { return _topics; }
            set { Set(() => Topics, ref _topics, value); }
        }

        private ModulesLookup _selectedTopic;
        public ModulesLookup SelectedTopic
        {
            get { return _selectedTopic; }
            set { Set(() => SelectedTopic, ref _selectedTopic, value); }
        }

        public TopicsVM(IEnumerable<ModulesLookup> topics)
        {
            Topics = new ObservableCollection<ModulesLookup>(topics);
        }

        protected override async Task LoadItems()
        {
            return;
        }
    }
}
