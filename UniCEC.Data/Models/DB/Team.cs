﻿using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Team
    {
        public Team()
        {
            ParticipantInTeams = new HashSet<ParticipantInTeam>();
        }

        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int TeamRoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfStudentInTeam { get; set; }
        public string InvitedCode { get; set; }
        public TeamStatus Status { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual TeamRole TeamRole { get; set; }
        public virtual ICollection<ParticipantInTeam> ParticipantInTeams { get; set; }
    }
}
