using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Image
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public string Alt { get; set; }
        public string Src { get; set; }
        public string Description { get; set; }

        public virtual Album Album { get; set; }
    }
}
