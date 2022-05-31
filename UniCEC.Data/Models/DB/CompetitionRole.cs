using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionRole
    {
        public CompetitionRole()
        {
            CompetitionManagers = new HashSet<CompetitionManager>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<CompetitionManager> CompetitionManagers { get; set; }
    }
}
