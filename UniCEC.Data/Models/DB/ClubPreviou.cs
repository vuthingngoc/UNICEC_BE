using System;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class ClubPreviou
    {
        public int Id { get; set; }
        public int ClubRoleId { get; set; }
        public int ClubId { get; set; }
        public int MemberId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Year { get; set; }
        public ClubPreviousStatus Status { get; set; }

        public virtual Club Club { get; set; }
        public virtual ClubRole ClubRole { get; set; }
        public virtual Member Member { get; set; }
    }
}
