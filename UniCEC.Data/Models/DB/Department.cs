using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Department
    {
        public Department()
        {
            CompetitionInDepartments = new HashSet<CompetitionInDepartment>();
            DepartmentInUniversities = new HashSet<DepartmentInUniversity>();
            Majors = new HashSet<Major>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CompetitionInDepartment> CompetitionInDepartments { get; set; }
        public virtual ICollection<DepartmentInUniversity> DepartmentInUniversities { get; set; }
        public virtual ICollection<Major> Majors { get; set; }
    }
}
