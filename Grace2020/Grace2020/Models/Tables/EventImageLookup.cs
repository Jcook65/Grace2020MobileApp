using Grace2020.Resources;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.Models.Tables
{
    [Table("EventsImagesLookup")]
    public class EventImageLookup : IModel
    {
        [PrimaryKey]
        public long LookupId { get; set; }
        public string ImageName { get; set; }
        [ForeignKey(typeof(Event))]
        public long EventId { get; set; }

        [Ignore]
        public string ImageURL
        {
            get { return string.Format(Constants.GRACE2020EventImageUrl, ImageName); }
        }
    }
}
