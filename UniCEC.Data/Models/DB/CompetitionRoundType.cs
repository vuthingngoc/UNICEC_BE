using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionRoundType
    {
        public CompetitionRoundType()
        {
            CompetitionRounds = new HashSet<CompetitionRound>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<CompetitionRound> CompetitionRounds { get; set; }
    }
}
