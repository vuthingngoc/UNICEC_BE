using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionManager
    {
        public int Id { get; set; }
        public int CompetitionInClubId { get; set; }
        public int CompetitionRoleId { get; set; }
        public int MemberId { get; set; }
        public string Fullname { get; set; }

        public virtual CompetitionInClub CompetitionInClub { get; set; }
        public virtual CompetitionRole CompetitionRole { get; set; }
        public virtual Member Member { get; set; }
    }
}
