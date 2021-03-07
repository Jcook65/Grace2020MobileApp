using Grace2020.Resources;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Grace2020.Models.Tables
{
    [Table("Prayers")]
    public class Prayer : IModel
    {
        [PrimaryKey]
        public string PrayerId { get; set; }
        [ForeignKey(typeof(Topic))]
        public string TopicId { get; set; }
        public string PrayerText { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.None)]
        public Topic Topic { get; set; }
    }
}
