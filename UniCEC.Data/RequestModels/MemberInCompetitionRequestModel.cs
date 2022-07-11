using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class MemberInCompetitionRequestModel : PagingRequest
    {
        //Competition Id
        [FromQuery(Name = "competitionId")]
        public int CompetitionId { get; set; }
        //Club Id
        [FromQuery(Name = "clubId")]
        public int ClubId { get; set; }
    }
}
