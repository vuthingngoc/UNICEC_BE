using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Department
    {
        public Department()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public int MajorId { get; set; }
        public int UniversityId { get; set; }
        public string Name { get; set; }
        public string DepartmentCode { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        public virtual Major Major { get; set; }
        public virtual University University { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
