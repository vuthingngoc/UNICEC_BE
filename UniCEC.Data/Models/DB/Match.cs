using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

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
        public string Address { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfTeam { get; set; }
        public MatchStatus Status { get; set; }
        public bool? IsLoseMatch { get; set; }

        public virtual CompetitionRound Round { get; set; }
        public virtual ICollection<TeamInMatch> TeamInMatches { get; set; }
    }
}
