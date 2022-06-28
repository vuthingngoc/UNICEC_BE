using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class EntityType
    {
        public EntityType()
        {
            CompetitionEntities = new HashSet<CompetitionEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CompetitionEntity> CompetitionEntities { get; set; }
    }
}
