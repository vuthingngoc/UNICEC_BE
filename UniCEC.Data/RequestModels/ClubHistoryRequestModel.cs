using Microsoft.AspNetCore.Mvc;
using System;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public  class ClubHistoryRequestModel : PagingRequest
    {
        [FromQuery(Name = "clubRoleId")]
        public int? ClubRoleId { get; set; }
        [FromQuery(Name = "memberId")]
        public int? MemberId { get; set; }
        [FromQuery(Name = "termId")]
        public int? TermId { get; set; }
        [FromQuery(Name = "startTime")]
        public DateTime? StartTime { get; set; }
        [FromQuery(Name = "endTime")]
        public DateTime? EndTime { get; set; }
        [FromQuery(Name = "status")]
        public ClubHistoryStatus? Status { get; set; }
    }
}
