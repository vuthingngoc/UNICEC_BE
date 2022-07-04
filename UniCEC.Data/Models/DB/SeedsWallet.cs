using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class SeedsWallet
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public double Amount { get; set; }
        public bool Status { get; set; }

        public virtual User Student { get; set; }
    }
}
