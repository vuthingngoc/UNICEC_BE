using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionRole
    {
        public CompetitionRole()
        {
            MemberInCompetitions = new HashSet<MemberInCompetition>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<MemberInCompetition> MemberInCompetitions { get; set; }
    }
}
