using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class CompetitionActivityRequestModel : PagingRequest
    {

        //Competition Id
        [FromQuery(Name = "competitionId"),BindRequired]
        public int? CompetitionId { get; set; }

        [FromQuery(Name = "priorityStatus")]
        public PriorityStatus? PriorityStatus { get; set; }

        [FromQuery(Name = "statuses")]
        public List<CompetitionActivityStatus> Statuses { get; set; }

        [FromQuery(Name = "name")]
        public string name { get; set; }

        //Club Id
        [FromQuery(Name = "clubId"), BindRequired]
        public int ClubId { get; set; }

    }
}
