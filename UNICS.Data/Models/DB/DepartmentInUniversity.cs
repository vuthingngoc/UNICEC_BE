using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class DepartmentInUniversity
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int UniversityId { get; set; }

        public virtual Department Department { get; set; }
        public virtual University University { get; set; }
    }
}
