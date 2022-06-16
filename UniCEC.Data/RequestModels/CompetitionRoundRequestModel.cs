using Microsoft.AspNetCore.Mvc;
using System;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class CompetitionRoundRequestModel : PagingRequest
    {
        [FromQuery(Name = "competitionId")]
        public int? CompetitionId { get; set; }
        [FromQuery(Name = "title")]
        public string Title { get; set; }
        [FromQuery(Name = "startTime")]
        public DateTime? StartTime { get; set; }
        [FromQuery(Name = "endTime")]
        public DateTime? EndTime { get; set; }
    }
}
