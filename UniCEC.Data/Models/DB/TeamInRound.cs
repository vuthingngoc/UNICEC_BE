using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class TeamInRound
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int RoundId { get; set; }
        public bool Status { get; set; }
        public int Rank { get; set; }
        public int Scores { get; set; }

        public virtual CompetitionRound Round { get; set; }
        public virtual Team Team { get; set; }
    }
}
