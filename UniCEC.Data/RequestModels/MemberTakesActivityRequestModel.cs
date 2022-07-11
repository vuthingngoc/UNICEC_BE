using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class MemberTakesActivityRequestModel : PagingRequest
    {
      
        [FromQuery(Name = "competitionActivityId")]
        public int CompetitionActivityId { get; set; }
        
        
        //Check
        [FromQuery(Name = "clubId")]
        public int ClubId { get; set; }

    }
}
