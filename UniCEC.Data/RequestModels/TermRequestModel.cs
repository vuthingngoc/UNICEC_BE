using Microsoft.AspNetCore.Mvc;
using System;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class TermRequestModel : PagingRequest
    {
        [FromQuery(Name = "createTime")]
        public DateTime? CreateTime { get; set; }
        [FromQuery(Name = "endTime")]
        public DateTime? EndTime { get; set; }
    }
}
