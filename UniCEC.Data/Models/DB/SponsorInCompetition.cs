using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class SponsorInCompetition
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int SponsorId { get; set; }
        public int UserId { get; set; }
        public SponsorInCompetitionStatus Status { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual Sponsor Sponsor { get; set; }
    }
}
