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
            CompetitionActivities = new HashSet<CompetitionActivity>();
            CompetitionEntities = new HashSet<CompetitionEntity>();
            CompetitionInClubs = new HashSet<CompetitionInClub>();
            CompetitionInDepartments = new HashSet<CompetitionInDepartment>();
            InfluencerInCompetitions = new HashSet<InfluencerInCompetition>();
            Participants = new HashSet<Participant>();
            SponsorInCompetitions = new HashSet<SponsorInCompetition>();
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        public int CompetitionTypeId { get; set; }
        public string Name { get; set; }
        public string AddressName { get; set; }
        public string Address { get; set; }
        public int NumberOfParticipation { get; set; }
        public int NumberOfTeam { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime StartTimeRegister { get; set; }
        public DateTime EndTimeRegister { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Content { get; set; }
        public double Fee { get; set; }
        public string SeedsCode { get; set; }
        public double SeedsPoint { get; set; }
        public double SeedsDeposited { get; set; }
        public int View { get; set; }
        public bool Public { get; set; }
        public bool IsSponsor { get; set; }
        public CompetitionStatus Status { get; set; }

        public virtual CompetitionType CompetitionType { get; set; }
        public virtual ICollection<CompetitionActivity> CompetitionActivities { get; set; }
        public virtual ICollection<CompetitionEntity> CompetitionEntities { get; set; }
        public virtual ICollection<CompetitionInClub> CompetitionInClubs { get; set; }
        public virtual ICollection<CompetitionInDepartment> CompetitionInDepartments { get; set; }
        public virtual ICollection<InfluencerInCompetition> InfluencerInCompetitions { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<SponsorInCompetition> SponsorInCompetitions { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
