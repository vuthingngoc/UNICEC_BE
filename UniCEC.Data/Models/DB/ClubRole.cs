using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class ClubRole
    {
        public ClubRole()
        {
            ClubPrevious = new HashSet<ClubPreviou>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ClubPreviou> ClubPrevious { get; set; }
    }
}
