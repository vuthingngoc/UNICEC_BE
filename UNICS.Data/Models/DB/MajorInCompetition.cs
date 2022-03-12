using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class MajorInCompetition
    {
        public int Id { get; set; }
        public int MajorId { get; set; }
        public int CompetitionId { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual Major Major { get; set; }
    }
}
