using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Term
{
    public class TermUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime? CreateTime { get; set; }
        [JsonPropertyName("end_time")]
        public DateTime? EndTime { get; set; }
    }
}
