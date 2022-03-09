using System;

namespace UNICS.Data.ViewModels.Entities.Album
{
    public class AlbumUpdateModel
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
