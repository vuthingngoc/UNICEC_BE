using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class MajorRequestModel : PagingRequest
    {
        [FromQuery(Name = "department-id")]
        public int? DepartmentId { get; set; }
        [FromQuery(Name = "major-code")]
        public string MajorCode { get; set; }
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
    }
}
