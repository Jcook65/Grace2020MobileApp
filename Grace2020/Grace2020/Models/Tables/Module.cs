using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.Models.Tables
{
    [Table("Modules")]
    public class Module : IModel
    {
        [PrimaryKey]
        public long ModuleId { get; set; }
        public DateTime? ActiveDate { get; set; }
        public DateTime? DeactiveDate { get; set; }
        public string ModuleName { get; set; }
    }
}
