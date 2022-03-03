using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class University
    {
        public University()
        {
            Campuses = new HashSet<Campus>();
            GroupUniversities = new HashSet<GroupUniversity>();
            Users = new HashSet<User>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public DateTime? Founding { get; set; }
        public DateTime? Openning { get; set; }
        public DateTime? Closing { get; set; }
        public int? Status { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Campus> Campuses { get; set; }
        public virtual ICollection<GroupUniversity> GroupUniversities { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
