using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Member
    {
        public Member()
        {
            ClubPrevious = new HashSet<ClubPreviou>();
            MemberTakesActivities = new HashSet<MemberTakesActivity>();
            Participants = new HashSet<Participant>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<ClubPreviou> ClubPrevious { get; set; }
        public virtual ICollection<MemberTakesActivity> MemberTakesActivities { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
