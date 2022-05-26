using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionEntity
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual Competition Competition { get; set; }
    }
}
