using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class TokenWallet
    {
        public string Id { get; set; }
        public double? Ammount { get; set; }
        public string StudentId { get; set; }

        public virtual User Student { get; set; }
    }
}
