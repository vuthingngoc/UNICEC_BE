using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Album
    {
        public Album()
        {
            Images = new HashSet<Image>();
            Videos = new HashSet<Video>();
        }

        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int AlbumTypeId { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual AlbumType AlbumType { get; set; }
        public virtual Competition Competition { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
    }
}
