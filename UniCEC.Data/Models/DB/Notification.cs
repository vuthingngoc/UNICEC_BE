using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public int UserId { get; set; }
        public bool IsAndroidDevice { get; set; }

        public virtual User User { get; set; }
    }
}
