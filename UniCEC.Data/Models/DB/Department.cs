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
            Majors = new HashSet<Major>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<CompetitionInDepartment> CompetitionInDepartments { get; set; }
        public virtual ICollection<Major> Majors { get; set; }
    }
}
