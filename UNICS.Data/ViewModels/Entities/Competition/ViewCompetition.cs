using System;

namespace UNICS.Data.ViewModels.Entities.Competition
{
    public class ViewCompetition
    {
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
        public string SeedsCode { get; set; }
        public double SeedsPoint { get; set; }
        public double SeedsDeposited { get; set; }
        public int View { get; set; }
    }
}
