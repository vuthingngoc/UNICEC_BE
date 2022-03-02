using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Area
    {
        public Area()
        {
            Campuses = new HashSet<Campus>();
        }

        public int Id { get; set; }
        public string City { get; set; }

        public virtual ICollection<Campus> Campuses { get; set; }
    }
}
