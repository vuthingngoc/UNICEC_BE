using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class Major
    {
        public Major()
        {
            MajorInCompetitions = new HashSet<MajorInCompetition>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public int MajorTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public virtual MajorType MajorType { get; set; }
        public virtual ICollection<MajorInCompetition> MajorInCompetitions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
