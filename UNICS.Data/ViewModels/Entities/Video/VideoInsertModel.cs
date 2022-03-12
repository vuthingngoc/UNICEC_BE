using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Video
{
    public class VideoInsertModel
    {
        [JsonPropertyName("album_id")]
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string Src { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
