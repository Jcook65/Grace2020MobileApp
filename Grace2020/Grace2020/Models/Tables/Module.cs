using Grace2020.Utils;
using Newtonsoft.Json;
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
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public DateTime? ActiveDate { get; set; }
        public DateTime? DeactiveDate { get; set; }
        public string OptionalSequenceDenotation { get; set; }
        [JsonConverter(typeof(BooleanJsonConverter))]
        public bool IsDateDependent { get; set; }
    }
}
