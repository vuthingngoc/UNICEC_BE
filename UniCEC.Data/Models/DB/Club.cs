using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Club
    {
        public Club()
        {
            CompetitionInClubs = new HashSet<CompetitionInClub>();
            Members = new HashSet<Member>();
        }

        public int Id { get; set; }
        public int UniversityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalMember { get; set; }
        public DateTime Founding { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        public string ClubFanpage { get; set; }
        public string ClubContact { get; set; }

        public virtual University University { get; set; }
        public virtual ICollection<CompetitionInClub> CompetitionInClubs { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
