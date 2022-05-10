using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Competition
    {
        public Competition()
        {
            Blogs = new HashSet<Blog>();
            CompetitionEntities = new HashSet<CompetitionEntity>();
            CompetitionInDepartments = new HashSet<CompetitionInDepartment>();
            Participants = new HashSet<Participant>();
            SponsorInCompetitions = new HashSet<SponsorInCompetition>();
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CompetitionTypeId { get; set; }
        public int NumberOfParticipation { get; set; }
        public int NumberOfTeam { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTimeRegister { get; set; }
        public DateTime EndTimeRegister { get; set; }
        public string SeedsCode { get; set; }
        public double SeedsPoint { get; set; }
        public double SeedsDeposited { get; set; }
        public bool Public { get; set; }
        public CompetitionStatus Status { get; set; }
        public string Address { get; set; }
        public int View { get; set; }

        public virtual CompetitionType CompetitionType { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<CompetitionEntity> CompetitionEntities { get; set; }
        public virtual ICollection<CompetitionInDepartment> CompetitionInDepartments { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<SponsorInCompetition> SponsorInCompetitions { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
