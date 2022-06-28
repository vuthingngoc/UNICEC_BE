using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionInClub
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public bool IsOwner { get; set; }
        public int CompetitionId { get; set; }

        public virtual Club Club { get; set; }
        public virtual Competition Competition { get; set; }
    }
}
