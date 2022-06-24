using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionRound
    {
        public CompetitionRound()
        {
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
        public int Status { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual ICollection<TeamInRound> TeamInRounds { get; set; }
    }
}
