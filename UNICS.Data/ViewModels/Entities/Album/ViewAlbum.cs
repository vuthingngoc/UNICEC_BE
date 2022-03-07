using System;

namespace UNICS.Data.ViewModels.Entities.Album
{
    public class ViewAlbum
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int AlbumTypeId { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
