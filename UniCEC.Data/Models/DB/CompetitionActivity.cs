using System;
using System.Collections.Generic;
using UniCEC.Data.Enum;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class CompetitionActivity
    {
        public CompetitionActivity()
        {
            ActivitiesEntities = new HashSet<ActivitiesEntity>();
            MemberTakesActivities = new HashSet<MemberTakesActivity>();
        }

        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double SeedsPoint { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime Ending { get; set; }
        public int NumOfMember { get; set; }
        public PriorityStatus Priority { get; set; }
        public CompetitionActivityStatus Status { get; set; }

        public virtual Competition Competition { get; set; }
        public virtual ICollection<ActivitiesEntity> ActivitiesEntities { get; set; }
        public virtual ICollection<MemberTakesActivity> MemberTakesActivities { get; set; }
    }
}
