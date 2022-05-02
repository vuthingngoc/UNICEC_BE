using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class ClubHistory
    {
        public int Id { get; set; }
        public int ClubRoleId { get; set; }
        public int ClubId { get; set; }
        public int MemberId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ClubHistoryStatus Status { get; set; }
        public int TermId { get; set; }

        public virtual Club Club { get; set; }
        public virtual ClubRole ClubRole { get; set; }
        public virtual Member Member { get; set; }
        public virtual Term Term { get; set; }
    }
}
