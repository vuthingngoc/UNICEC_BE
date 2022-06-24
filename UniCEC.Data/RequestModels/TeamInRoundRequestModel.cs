using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class TeamInRoundRequestModel : PagingRequest
    {
        [FromQuery(Name = "roundId"), BindRequired]
        public int RoundId { get; set; }
        [FromQuery(Name = "top")]
        public int? Top { get; set; }
        [FromQuery(Name = "teamId")]
        public int? TeamId { get; set; }        
        [FromQuery(Name = "result")]
        public int? Scores { get; set; }
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
        [FromQuery(Name = "rank")]
        public int? Rank { get; set; }
    }
}
