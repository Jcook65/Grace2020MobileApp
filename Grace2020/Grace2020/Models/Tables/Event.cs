using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.Models.Tables
{
    [Table("Events")]
    public class Event : IModel
    {
        [PrimaryKey]
        public string EventId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? ActiveDate { get; set; }
        public DateTime? DeactiveDate { get; set; }
        public string URL { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }

        [Ignore]
        public string FormattedDate
        {
            get
            {
                if(EventStartDate == EventEndDate)
                {
                    return EventStartDate.ToString("dddd, MMMM dd", App.GetCultureInfo());
                }
                else
                {
                    return $"{EventStartDate.ToString("dddd, MMMM dd", App.GetCultureInfo())} - {EventEndDate.ToString("dddd, MMMM dd", App.GetCultureInfo())}";
                }
            }
        }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public EventAssetLookup EventImage { get; set; }
    }
}
