using Grace2020.Resources;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Grace2020.Models.Tables
{
    [Table("ModulesLookups")]
    public class ModulesLookup : IModel, INotifyPropertyChanged
    {
        [PrimaryKey]
        public long ModuleLookupId { get; set; }
        public long ModuleId { get; set; }
        [ForeignKey(typeof(Prayer))]
        public long PrayerId { get; set; }
        public DateTime? Date { get; set; }
        public int Sequence { get; set; }
        public string Title { get; set; }

        public string ImageName { get; set; }


        private bool _isSelected;
        [Ignore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
        }

        [Ignore]
        public string FullTitle { get { return Title != null ? $"{Title} {Sequence}" : null; } }
        [Ignore]
        public string ImageURL { get { return string.Format(Constants.GRACE2020RegionImageUrl, ImageName); } }

        [OneToOne(CascadeOperations = CascadeOperation.None)]
        public Prayer Prayer { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
