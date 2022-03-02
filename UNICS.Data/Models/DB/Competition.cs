using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Competition
    {
        public Competition()
        {
            Albums = new HashSet<Album>();
            Blogs = new HashSet<Blog>();
            Comments = new HashSet<Comment>();
            GroupUniversities = new HashSet<GroupUniversity>();
            Majors = new HashSet<Major>();
            Ratings = new HashSet<Rating>();
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        public int CompetitionTypeId { get; set; }
        public string Organizer { get; set; }
        public int NumberOfParticipations { get; set; }
        public int NumberOfGroups { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTimeRegister { get; set; }
        public DateTime EndTimeRegister { get; set; }
        public DateTime ApprovedTime { get; set; }
        public bool Public { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }
        public string TokenCode { get; set; }
        public double TokenPoint { get; set; }
        public double TokenDeposited { get; set; }
        public int View { get; set; }

        public virtual CompetitionType CompetitionType { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GroupUniversity> GroupUniversities { get; set; }
        public virtual ICollection<Major> Majors { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
