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
        public string TitleText { get; set; }
        public string NewsText { get; set; }

        [OneToMany]
        public List<ImageLookup> ImageLookups { get; set; }

        [Ignore]
        public string FormattedDate { get { return Date.ToString("MMMM dd", App.GetCultureInfo()); } }
        [Ignore]
        public string TruncatedNews { get { return string.Concat(NewsText.Substring(0, NewsText.Length > 185 ? 185 : (int)(NewsText.Length * .25)).TrimEnd(), "..."); } }
        [Ignore]
        public List<SelectableItemVM> Images 
        { 
            get
            {
                return ImageLookups?.Select(i => new SelectableItemVM(i.ImageName))?.ToList();
            } 
        }

        [Ignore]
        public string ImageSourceURL
        {
            get 
            { 
                var img = Images?.FirstOrDefault();
                return img != null ? string.Format(Constants.GRACE2020NewsImageUrl, img.Item as string) : "";
            }
        }
    }
}
