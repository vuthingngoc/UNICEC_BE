using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Album
{
    public class AlbumUpdateModel
    {
        public int Id { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
