using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Term
    {
        public Term()
        {
            ClubHistories = new HashSet<ClubHistory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual ICollection<ClubHistory> ClubHistories { get; set; }
    }
}
