using Microsoft.AspNetCore.Mvc;
using System;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class TermRequestModel : PagingRequest
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "create-time")]
        public DateTime? CreateTime { get; set; }
        [FromQuery(Name = "end-time")]
        public DateTime? EndTime { get; set; }
    }
}
