using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class University
    {
        public University()
        {
            Clubs = new HashSet<Club>();
            Competitions = new HashSet<Competition>();
            Departments = new HashSet<Department>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public int CityId { get; set; }
        public string UniCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Founding { get; set; }
        public string Openning { get; set; }
        public string Closing { get; set; }
        public bool Status { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Club> Clubs { get; set; }
        public virtual ICollection<Competition> Competitions { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
