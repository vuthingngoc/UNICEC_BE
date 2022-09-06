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
            CompetitionHistories = new HashSet<CompetitionHistory>();
            CompetitionInClubs = new HashSet<CompetitionInClub>();
            CompetitionInMajors = new HashSet<CompetitionInMajor>();
            CompetitionRounds = new HashSet<CompetitionRound>();
            MemberInCompetitions = new HashSet<MemberInCompetition>();
            Participants = new HashSet<Participant>();
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        public int CompetitionTypeId { get; set; }
        public int UniversityId { get; set; }
        public string Name { get; set; }
        public string AddressName { get; set; }
        public string Address { get; set; }
        public int NumberOfParticipation { get; set; }
        public int? NumberOfTeam { get; set; }
        public int? MinNumber { get; set; }
        public int? MaxNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime StartTimeRegister { get; set; }
        public DateTime EndTimeRegister { get; set; }
        public DateTime CeremonyTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Content { get; set; }
        public double Fee { get; set; }
        public string SeedsCode { get; set; }
        public double SeedsPoint { get; set; }
        public double SeedsDeposited { get; set; }
        public int View { get; set; }
        public CompetitionScopeStatus Scope { get; set; }
        public bool IsSponsor { get; set; }
        public CompetitionStatus Status { get; set; }
        public int RequiredMin { get; set; }

        public virtual CompetitionType CompetitionType { get; set; }
        public virtual University University { get; set; }
        public virtual ICollection<CompetitionActivity> CompetitionActivities { get; set; }
        public virtual ICollection<CompetitionEntity> CompetitionEntities { get; set; }
        public virtual ICollection<CompetitionHistory> CompetitionHistories { get; set; }
        public virtual ICollection<CompetitionInClub> CompetitionInClubs { get; set; }
        public virtual ICollection<CompetitionInMajor> CompetitionInMajors { get; set; }
        public virtual ICollection<CompetitionRound> CompetitionRounds { get; set; }
        public virtual ICollection<MemberInCompetition> MemberInCompetitions { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
