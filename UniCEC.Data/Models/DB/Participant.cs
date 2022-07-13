using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Participant
    {
        public Participant()
        {
            ParticipantInTeams = new HashSet<ParticipantInTeam>();
        }

        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CompetitionId { get; set; }
        public DateTime RegisterTime { get; set; }
        public bool Attendance { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual User Student { get; set; }
        public virtual ICollection<ParticipantInTeam> ParticipantInTeams { get; set; }
    }
}
