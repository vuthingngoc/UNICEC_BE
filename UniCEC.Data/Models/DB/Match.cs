using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Match
    {
        public Match()
        {
            TeamInMatches = new HashSet<TeamInMatch>();
        }

        public int Id { get; set; }
        public int RoundId { get; set; }
        public int MatchTypeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Scores { get; set; }
        public int Status { get; set; }

        public virtual MatchType MatchType { get; set; }
        public virtual CompetitionRound Round { get; set; }
        public virtual ICollection<TeamInMatch> TeamInMatches { get; set; }
    }
}
