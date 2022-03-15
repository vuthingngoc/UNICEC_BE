using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Image
{
    public class ViewImage
    {
        public int Id { get; set; }
        [JsonPropertyName("album_id")]
        public int AlbumId { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }
        public string Alt { get; set; }
        public string Src { get; set; }
        public string Description { get; set; }
    }
}
