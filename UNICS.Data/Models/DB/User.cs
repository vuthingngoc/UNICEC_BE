using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class User
    {
        public User()
        {
            Blogs = new HashSet<Blog>();
            Comments = new HashSet<Comment>();
            Participants = new HashSet<Participant>();
            Ratings = new HashSet<Rating>();
            SeedsWallets = new HashSet<SeedsWallet>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public int? UniversityId { get; set; }
        public int? MajorId { get; set; }
        public string Fullname { get; set; }
        public string StudentId { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int? Status { get; set; }
        public string Dob { get; set; }
        public string Description { get; set; }

        public virtual Major Major { get; set; }
        public virtual Role Role { get; set; }
        public virtual University University { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<SeedsWallet> SeedsWallets { get; set; }
    }
}
