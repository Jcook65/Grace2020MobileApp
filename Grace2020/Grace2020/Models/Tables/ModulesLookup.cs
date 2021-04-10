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
        public string ModuleLookupId { get; set; }
        [ForeignKey(typeof(Module))]
        public string ModuleId { get; set; }
        [ForeignKey(typeof(Topic))]
        public string TopicId { get; set; }
        public DateTime? Date { get; set; }
        public int Sequence { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.None)]
        public Topic Topic { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.None)]
        public Module Module { get; set; }

        [Ignore]
        public string OptionalDelimiter => Module?.OptionalSequenceDenotation != null 
            ? App.GetCultureInfo().TwoLetterISOLanguageName.ToLower() == "ja" 
            ? $"{Sequence} {Module.OptionalSequenceDenotation}" : $"{Module.OptionalSequenceDenotation} {Sequence}" : null;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
