using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class ClubRole
    {
        public ClubRole()
        {
            Members = new HashSet<Member>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}
