using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
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
        public int? SponsorId { get; set; }
        public int? UniversityId { get; set; }
        public int? MajorId { get; set; }
        public string Fullname { get; set; }
        public string StudentCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public UserStatus Status { get; set; }
        public bool IsOnline { get; set; }

        public virtual Department Major { get; set; }
        public virtual Role Role { get; set; }
        public virtual Sponsor Sponsor { get; set; }
        public virtual University University { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<SeedsWallet> SeedsWallets { get; set; }
    }
}
