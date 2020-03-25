using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.Models.Tables
{
    [Table("ImageLookups")]
    public class ImageLookup : IModel
    {
        [PrimaryKey]
        public string LookupId { get; set; }
        public string ImageName { get; set; }
        [ForeignKey(typeof(News))]
        public string NewsId { get; set; }
    }
}
