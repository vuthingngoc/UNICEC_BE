using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Blog
    {
        public int Id { get; set; }
        public int? CompetitionId { get; set; }
        public int BlogTypeId { get; set; }
        public int ClubId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }

        public virtual BlogType BlogType { get; set; }
        public virtual Club Club { get; set; }
        public virtual Competition Competition { get; set; }
    }
}
