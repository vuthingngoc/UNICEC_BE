using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class AlbumType
    {
        public AlbumType()
        {
            Albums = new HashSet<Album>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
