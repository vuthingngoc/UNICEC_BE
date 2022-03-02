using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Participant
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public string StudentId { get; set; }
        public DateTime? RegisterTime { get; set; }

        public virtual User Student { get; set; }
        public virtual Team Team { get; set; }
    }
}
