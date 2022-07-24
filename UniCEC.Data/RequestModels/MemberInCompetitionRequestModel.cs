using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class MemberInCompetitionRequestModel : PagingRequest
    {
        //Competition Id
        [FromQuery(Name = "competitionId"), BindRequired]
        public int CompetitionId { get; set; }
        //Club Id
        [FromQuery(Name = "clubId"), BindRequired]
        public int ClubId { get; set; }
    }
}
