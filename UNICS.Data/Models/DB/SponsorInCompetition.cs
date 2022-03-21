﻿using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class SponsorInCompetition
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int SponsorId { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual Sponsor Sponsor { get; set; }
    }
}
