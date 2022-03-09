using System;

namespace UNICS.Data.ViewModels.Entities.Participant
{
    public class ViewParticipant
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public int? StudentId { get; set; }
        public DateTime? RegisterTime { get; set; }
    }
}
