using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class MemberTakesActivity
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int BookerId { get; set; }
        public int CompetitionActivityId { get; set; }

        public virtual CompetitionActivity CompetitionActivity { get; set; }
        public virtual Member Member { get; set; }
    }
}
