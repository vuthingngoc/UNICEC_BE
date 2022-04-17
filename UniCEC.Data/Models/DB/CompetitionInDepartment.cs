using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionInDepartment
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int CompetitionId { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual Department Department { get; set; }
    }
}
