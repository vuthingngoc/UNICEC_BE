using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Image
{
    public class ImageInsertModel
    {
        [JsonPropertyName("album_id")]
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string Alt { get; set; }
        public string Src { get; set; }
        public string Description { get; set; }
    }
}
