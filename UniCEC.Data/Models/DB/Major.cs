using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Major
    {
        public Major()
        {
            CompetitionInMajors = new HashSet<CompetitionInMajor>();
            Departments = new HashSet<Department>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<CompetitionInMajor> CompetitionInMajors { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}
