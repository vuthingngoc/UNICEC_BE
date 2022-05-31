using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class TeamRole
    {
        public TeamRole()
        {
            ParticipantInTeams = new HashSet<ParticipantInTeam>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ParticipantInTeam> ParticipantInTeams { get; set; }
    }
}
