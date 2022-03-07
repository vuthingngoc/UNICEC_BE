using System;

namespace UNICS.Data.ViewModels.Image
{
    public class ViewImage
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public string Alt { get; set; }
        public string Src { get; set; }
        public string Description { get; set; }
    }
}
