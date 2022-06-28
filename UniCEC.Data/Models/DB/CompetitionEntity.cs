using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionEntity
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int EntityTypeId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual EntityType EntityType { get; set; }
    }
}
