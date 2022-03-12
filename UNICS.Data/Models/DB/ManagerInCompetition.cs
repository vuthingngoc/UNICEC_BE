using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class ManagerInCompetition
    {
        public int? CompetitionId { get; set; }
        public int? UserId { get; set; }
        public int Id { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual User User { get; set; }
    }
}
