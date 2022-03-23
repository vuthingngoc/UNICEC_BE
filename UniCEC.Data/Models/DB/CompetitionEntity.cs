using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionEntity
    {
        public int Id { get; set; }
        public int EntityTypeId { get; set; }
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual EntityType EntityType { get; set; }
    }
}
