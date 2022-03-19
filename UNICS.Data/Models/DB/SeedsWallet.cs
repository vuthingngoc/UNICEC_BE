using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class SeedsWallet
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public double Ammount { get; set; }

        public virtual User User { get; set; }
    }
}
