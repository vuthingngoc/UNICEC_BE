using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Member
    {
        public Member()
        {
            CompetitionManagers = new HashSet<CompetitionManager>();
            MemberTakesActivities = new HashSet<MemberTakesActivity>();
            Participants = new HashSet<Participant>();
        }

        public int Id { get; set; }
        public int ClubRoleId { get; set; }
        public int ClubId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public MemberStatus Status { get; set; }
        public int TermId { get; set; }

        public virtual Club Club { get; set; }
        public virtual ClubRole ClubRole { get; set; }
        public virtual Term Term { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CompetitionManager> CompetitionManagers { get; set; }
        public virtual ICollection<MemberTakesActivity> MemberTakesActivities { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
