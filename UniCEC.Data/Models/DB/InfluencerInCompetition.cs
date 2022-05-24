using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class InfluencerInCompetition
    {
        public int Id { get; set; }
        public int InfluencerId { get; set; }
        public int CompetitionId { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual Influencer Influencer { get; set; }
    }
}
