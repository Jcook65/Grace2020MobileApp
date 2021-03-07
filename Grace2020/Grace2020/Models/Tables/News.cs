using Grace2020.Resources;
using Grace2020.ViewModels.Instances;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grace2020.Models.Tables
{
    [Table("News")]
    public class News : IModel
    {
        [PrimaryKey]
        public string NewsId { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        [OneToMany]
        public List<NewsAssetLookup> Assets { get; set; }

        [Ignore]
        public string FormattedDate { get { return Date.ToString("MMMM dd", App.GetCultureInfo()); } }
        [Ignore]
        public string TruncatedNews { get { return string.Concat(Body.Substring(0, Body.Length > 185 ? 185 : (int)(Body.Length * .25)).TrimEnd(), "..."); } }
        [Ignore]
        public List<SelectableItemVM> Images 
        { 
            get
            {
                return Assets?.Select(i => new SelectableItemVM(i))?.ToList();
            } 
        }

        [Ignore]
        public string ImageSourceURL
        {
            get 
            { 
                return Assets.FirstOrDefault()?.AssetUrl;
            }
        }
    }
}
