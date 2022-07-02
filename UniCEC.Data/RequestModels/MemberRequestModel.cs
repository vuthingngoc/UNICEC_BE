using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public  class MemberRequestModel : PagingRequest
    {
        [FromQuery(Name = "clubId"), BindRequired]
        public int ClubId { get; set; }
        [FromQuery(Name = "searchString")]
        public string SearchString { get; set; }
        [FromQuery(Name = "clubRoleId")]
        public int? ClubRoleId { get; set; }
        [FromQuery(Name = "startTime")]
        public DateTime? StartTime { get; set; }
        [FromQuery(Name = "endTime")]
        public DateTime? EndTime { get; set; }
        [FromQuery(Name = "status")]
        public MemberStatus? Status { get; set; }
        public bool? IsOnline { get; set; }
    }
}
