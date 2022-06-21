using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class CompetitionActivityRequestModel : PagingRequest
    {

        //Competition Id
        [FromQuery(Name = "competitionId")]
        public int? CompetitionId { get; set; }

        [FromQuery(Name = "processStatus")]
        public  CompetitionActivityProcessStatus? ProcessStatus { get; set; }

        [FromQuery(Name = "priorityStatus")]
        public PriorityStatus? PriorityStatus { get; set; }

        public CompetitionActivityStatus? Status { get; set; }   

        //Club Id
        [FromQuery(Name = "clubId")]
        public int ClubId { get; set; }

    }
}
