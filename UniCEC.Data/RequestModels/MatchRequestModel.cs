using Microsoft.AspNetCore.Mvc;
using System;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class MatchRequestModel : PagingRequest
    {
        [FromQuery(Name = "competitionId")]
        public int? CompetitionId { get; set; }
        [FromQuery(Name = "roundId")]
        public int? RoundId { get; set; }
        [FromQuery(Name = "isLoseMatch")]
        public bool? IsLoseMatch { get; set; }
        [FromQuery(Name = "startTime")]
        public DateTime? StartTime { get; set; }
        [FromQuery(Name = "endTime")]
        public DateTime? EndTime { get; set; }
        [FromQuery(Name = "status")]
        public MatchStatus? Status { get; set; }
    }
}
