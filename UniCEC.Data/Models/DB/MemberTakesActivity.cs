using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class MemberTakesActivity
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int ClubActivityId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Deadline { get; set; }
        public MemberTakesActivityStatus Status { get; set; }

        public virtual CompetitionActivity ClubActivity { get; set; }
        public virtual Member Member { get; set; }
    }
}
