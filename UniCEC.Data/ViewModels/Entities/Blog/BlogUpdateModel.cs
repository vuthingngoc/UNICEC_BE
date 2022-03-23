using System;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Blog
{
    public class BlogUpdateModel
    {
        public int Id { get; set; }
        [JsonPropertyName("blog_type_id")]
        public int BlogTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
    }
}
