using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class TeamInMatch
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public int Scores { get; set; }
        public int Status { get; set; }

        public virtual Match Match { get; set; }
        public virtual Team Team { get; set; }
    }
}
