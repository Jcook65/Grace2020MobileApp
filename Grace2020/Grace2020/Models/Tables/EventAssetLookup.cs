using Grace2020.Resources;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.Models.Tables
{
    [Table("EventsImagesLookup")]
    public class EventAssetLookup : IModel
    {
        [PrimaryKey]
        public string LookupId { get; set; }
        public string AssetName { get; set; }
        [ForeignKey(typeof(Event))]
        public string EventId { get; set; }

        [Ignore]
        public string AssetUrl => $"{Constants.GRACE2020ImageUrl}?file={AssetName}";
    }
}
