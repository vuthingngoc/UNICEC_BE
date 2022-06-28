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
            MemberInCompetitions = new HashSet<MemberInCompetition>();
            MemberTakesActivities = new HashSet<MemberTakesActivity>();
        }

        public int Id { get; set; }
        public int ClubRoleId { get; set; }
        public int ClubId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public MemberStatus Status { get; set; }

        public virtual Club Club { get; set; }
        public virtual ClubRole ClubRole { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<MemberInCompetition> MemberInCompetitions { get; set; }
        public virtual ICollection<MemberTakesActivity> MemberTakesActivities { get; set; }
    }
}
