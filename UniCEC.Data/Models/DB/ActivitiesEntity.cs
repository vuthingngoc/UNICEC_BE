using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class ActivitiesEntity
    {
        public int Id { get; set; }
        public int ClubActivityId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public virtual ClubActivity ClubActivity { get; set; }
    }
}
