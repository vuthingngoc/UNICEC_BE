using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionType
    {
        public CompetitionType()
        {
            Competitions = new HashSet<Competition>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<Competition> Competitions { get; set; }
    }
}
