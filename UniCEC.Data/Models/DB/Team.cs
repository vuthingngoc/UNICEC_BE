using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Team
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfStudentInTeam { get; set; }
        public string InvitedCode { get; set; }
        public TeamStatus Status { get; set; }

        public virtual Competition Competition { get; set; }
    }
}
