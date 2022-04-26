using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Club
    {
        public Club()
        {
            Blogs = new HashSet<Blog>();
            ClubActivities = new HashSet<ClubActivity>();
            ClubHistories = new HashSet<ClubHistory>();
            CompetitionInClubs = new HashSet<CompetitionInClub>();
        }

        public int Id { get; set; }
        public int UniversityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalMember { get; set; }
        public DateTime Founding { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }

        public virtual University University { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<ClubActivity> ClubActivities { get; set; }
        public virtual ICollection<ClubHistory> ClubHistories { get; set; }
        public virtual ICollection<CompetitionInClub> CompetitionInClubs { get; set; }
    }
}
