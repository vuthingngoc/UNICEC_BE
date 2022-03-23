using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Sponsor
    {
        public Sponsor()
        {
            SponsorInCompetitions = new HashSet<SponsorInCompetition>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<SponsorInCompetition> SponsorInCompetitions { get; set; }
    }
}
