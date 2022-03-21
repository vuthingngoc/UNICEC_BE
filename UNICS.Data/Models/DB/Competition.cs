using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Competition
    {
        public Competition()
        {
            Blogs = new HashSet<Blog>();
            CompetitionEntities = new HashSet<CompetitionEntity>();
            CompetitionInClubs = new HashSet<CompetitionInClub>();
            Departments = new HashSet<Department>();
            Participants = new HashSet<Participant>();
            SponsorInCompetitions = new HashSet<SponsorInCompetition>();
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        public int CompetitionTypeId { get; set; }
        public string Organizer { get; set; }
        public int NumberOfParticipation { get; set; }
        public int NumberOfGroup { get; set; }
        public DateTime ApprovedTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTimeRegister { get; set; }
        public DateTime EndTimeRegister { get; set; }
        public string SeedsCode { get; set; }
        public double SeedsPoint { get; set; }
        public double SeedsDeposited { get; set; }
        public bool Public { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }
        public int View { get; set; }

        public virtual CompetitionType CompetitionType { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<CompetitionEntity> CompetitionEntities { get; set; }
        public virtual ICollection<CompetitionInClub> CompetitionInClubs { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<SponsorInCompetition> SponsorInCompetitions { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
