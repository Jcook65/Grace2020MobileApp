using Grace2020.Resources;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.Models.Tables
{
    [Table("Topics")]
    public class Topic : IModel
    {
        [PrimaryKey]
        public string TopicId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Info { get; set; }
        public string AssetName { get; set; }

        public string AssetUrl => $"{Constants.GRACE2020ImageUrl}?file={AssetName}";

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Prayer> Prayers { get; set; }
    }
}
