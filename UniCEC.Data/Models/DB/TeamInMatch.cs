using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class TeamInMatch
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public int Scores { get; set; }
        public TeamInMatchStatus Status { get; set; }
        public string Description { get; set; }

        public virtual Match Match { get; set; }
        public virtual Team Team { get; set; }
    }
}
