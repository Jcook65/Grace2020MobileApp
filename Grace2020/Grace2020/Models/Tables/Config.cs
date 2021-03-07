using SQLite;

namespace Grace2020.Models.Tables
{
    [Table("Config")]
    public class Config : IModel
    {
        [PrimaryKey]
        public int ConfigId { get; set; }
        public string CurrentModuleId { get; set; }
        public string CurrentTopicId { get; set; }
        public int AppTheme { get; set; }
    }
}
