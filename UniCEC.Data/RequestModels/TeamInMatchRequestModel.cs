using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class TeamInMatchRequestModel : PagingRequest
    {
        [FromQuery(Name = "roundId")]
        public int? RoundId { get; set; }
        [FromQuery(Name = "matchId")]
        public int? MatchId { get; set; }
        [FromQuery(Name = "teamId")]
        public int? TeamId { get; set; }
        [FromQuery(Name = "scores")]
        public int? Scores { get; set; }
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
