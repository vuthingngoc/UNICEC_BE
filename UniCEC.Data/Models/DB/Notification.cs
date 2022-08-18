using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual User User { get; set; }
    }
}
