using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Album
{
    public class AlbumInsertModel
    {
        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }
        [JsonPropertyName("album_type_id")]
        public int AlbumTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
