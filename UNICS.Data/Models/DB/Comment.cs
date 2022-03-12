using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CompetitionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual User Student { get; set; }
    }
}
