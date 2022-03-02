using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Rating
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public int CompetitionId { get; set; }
        public int Rate { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual User Student { get; set; }
    }
}
