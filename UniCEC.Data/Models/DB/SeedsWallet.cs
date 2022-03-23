using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class SeedsWallet
    {
        public string Id { get; set; }
        public int StudentId { get; set; }
        public double Ammount { get; set; }

        public virtual User Student { get; set; }
    }
}
