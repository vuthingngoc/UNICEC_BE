using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class User
    {
        public User()
        {
            Members = new HashSet<Member>();
            Participants = new HashSet<Participant>();
            SeedsWallets = new HashSet<SeedsWallet>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public int? UniversityId { get; set; }
        public int? MajorId { get; set; }
        public string Fullname { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool Status { get; set; }
        public string Dob { get; set; }
        public string Description { get; set; }

        public virtual Major Major { get; set; }
        public virtual Role Role { get; set; }
        public virtual University University { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<SeedsWallet> SeedsWallets { get; set; }
    }
}
