using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionHistory
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int? ChangerId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Description { get; set; }
        public CompetitionStatus Status { get; set; }

        public virtual Competition Competition { get; set; }
    }
}
