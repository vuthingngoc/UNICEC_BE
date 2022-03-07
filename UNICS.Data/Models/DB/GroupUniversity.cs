using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class GroupUniversity
    {
        public int Id { get; set; }
        public int? UniversityId { get; set; }
        public int? CompetitionId { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual University University { get; set; }
    }
}
