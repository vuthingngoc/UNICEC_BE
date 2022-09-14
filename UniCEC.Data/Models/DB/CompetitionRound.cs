using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionRound
    {
        public CompetitionRound()
        {
            Matches = new HashSet<Match>();
            TeamInRounds = new HashSet<TeamInRound>();
        }

        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfTeam { get; set; }
        public int SeedsPoint { get; set; }
        public CompetitionRoundStatus Status { get; set; }
        public int Order { get; set; }
        public int CompetitionRoundTypeId { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual CompetitionRoundType CompetitionRoundType { get; set; }
        public virtual ICollection<Match> Matches { get; set; }
        public virtual ICollection<TeamInRound> TeamInRounds { get; set; }
    }
}
