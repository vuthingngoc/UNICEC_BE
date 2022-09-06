using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class MatchType
    {
        public MatchType()
        {
            Matches = new HashSet<Match>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
