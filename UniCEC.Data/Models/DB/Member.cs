using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Member
    {
        public Member()
        {
            ClubHistories = new HashSet<ClubHistory>();
            MemberTakesActivities = new HashSet<MemberTakesActivity>();
            Participants = new HashSet<Participant>();
        }

        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime JoinDate { get; set; }

        public virtual User Student { get; set; }
        public virtual ICollection<ClubHistory> ClubHistories { get; set; }
        public virtual ICollection<MemberTakesActivity> MemberTakesActivities { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
