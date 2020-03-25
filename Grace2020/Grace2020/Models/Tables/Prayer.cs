using Grace2020.Resources;
using SQLite;
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
        public long PrayerId { get; set; }
        public string Region { get; set; }
        public string TitleText { get; set; }
        public string PrayerText { get; set; }
        public string MoreInfoText { get; set; }

        public string PrefectureFormat { get { return $"{TitleText} {StringResources.Prefecture}"; } }
        public string RegionFormat { get { return $"{Region} {StringResources.Region}"; } }

        public string FormattedPrayer 
        { 
            get
            {
                return PrayerText.Replace("||", "\n\n");
            }
        }

        public List<string> Prayers
        {
            get 
            { 
                return PrayerText.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToList();
            }
        }
    }
}
