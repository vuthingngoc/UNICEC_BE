﻿using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Participant
    {
        public Participant()
        {
            ParticipantInTeams = new HashSet<ParticipantInTeam>();
        }

        public int Id { get; set; }
        public int MemberId { get; set; }
        public int UserId { get; set; }
        public int CompetitionId { get; set; }
        public DateTime RegisterTime { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual Member Member { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ParticipantInTeam> ParticipantInTeams { get; set; }
    }
}
