using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Campus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public string Address { get; set; }
        public int? UniversityId { get; set; }
        public int? AreaId { get; set; }

        public virtual Area Area { get; set; }
        public virtual University University { get; set; }
    }
}
