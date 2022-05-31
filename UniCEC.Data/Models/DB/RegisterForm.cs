using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class RegisterForm
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public virtual Club Club { get; set; }
    }
}
