using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Influencer
    {
        public Influencer()
        {
            InfluencerInCompetitions = new HashSet<InfluencerInCompetition>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<InfluencerInCompetition> InfluencerInCompetitions { get; set; }
    }
}
