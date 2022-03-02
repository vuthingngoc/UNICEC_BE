using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class MajorType
    {
        public MajorType()
        {
            Majors = new HashSet<Major>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Major> Majors { get; set; }
    }
}
