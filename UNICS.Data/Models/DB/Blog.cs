using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? Status { get; set; }
        public int? UserId { get; set; }
        public int CompetitionId { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual User User { get; set; }
    }
}
