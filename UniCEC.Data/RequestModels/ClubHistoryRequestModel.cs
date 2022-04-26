using Microsoft.AspNetCore.Mvc;
using System;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public  class ClubHistoryRequestModel : PagingRequest
    {
        [FromQuery(Name = "club-role-id")]
        public int? ClubRoleId { get; set; }
        [FromQuery(Name = "club-id")]
        public int? ClubId { get; set; }
        [FromQuery(Name = "member-id")]
        public int? MemberId { get; set; }
        [FromQuery(Name = "term-id")]
        public int? TermId { get; set; }
        [FromQuery(Name = "start-time")]
        public DateTime? StartTime { get; set; }
        [FromQuery(Name = "end-time")]
        public DateTime? EndTime { get; set; }
        public int? Status { get; set; }
    }
}
