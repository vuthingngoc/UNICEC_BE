using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class ClubActivity
    {
        public ClubActivity()
        {
            ActivitiesEntities = new HashSet<ActivitiesEntity>();
            MemberTakesActivities = new HashSet<MemberTakesActivity>();
        }

        public int Id { get; set; }
        public int ClubId { get; set; }
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double SeedsPoint { get; set; }
        public string SeedsCode { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime Ending { get; set; }
        public int NumOfMember { get; set; }
        public int Priority { get; set; }
        public ClubActivityStatus Status { get; set; }

        public virtual Club Club { get; set; }
        public virtual Competition Competition { get; set; }
        public virtual ICollection<ActivitiesEntity> ActivitiesEntities { get; set; }
        public virtual ICollection<MemberTakesActivity> MemberTakesActivities { get; set; }
    }
}
