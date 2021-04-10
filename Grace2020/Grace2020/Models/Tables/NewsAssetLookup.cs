using Grace2020.Resources;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Grace2020.Models.Tables
{
    [Table("ImageLookups")]
    public class NewsAssetLookup : IModel
    {
        [PrimaryKey]
        public string LookupId { get; set; }
        public string AssetName { get; set; }
        [ForeignKey(typeof(News))]
        public string NewsId { get; set; }

        [Ignore]
        public string AssetUrl => $"{Constants.GRACE2020ImageUrl}?file={HttpUtility.UrlEncode(AssetName)}";
    }
}
