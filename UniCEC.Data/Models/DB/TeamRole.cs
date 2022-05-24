using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class TeamRole
    {
        public TeamRole()
        {
            ParticipantInTeamTeamRoles = new HashSet<ParticipantInTeam>();
            ParticipantInTeamTeams = new HashSet<ParticipantInTeam>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ParticipantInTeam> ParticipantInTeamTeamRoles { get; set; }
        public virtual ICollection<ParticipantInTeam> ParticipantInTeamTeams { get; set; }
    }
}
