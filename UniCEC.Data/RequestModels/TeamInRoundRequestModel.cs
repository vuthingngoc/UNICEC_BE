using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class TeamInRoundRequestModel : PagingRequest
    {
        [FromQuery(Name = "teamId")]
        public int TeamId { get; set; }
        [FromQuery(Name = "roundId")]
        public int RoundId { get; set; }
        [FromQuery(Name = "result")]
        public string Result { get; set; }
        [FromQuery(Name = "status")]
        public bool Status { get; set; }
        [FromQuery(Name = "rank")]
        public int Rank { get; set; }
    }
}
