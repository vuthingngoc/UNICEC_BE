using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class MajorRequestModel : PagingRequest
    {
        [FromQuery(Name = "department-id")]
        public int? DepartmentId { get; set; }
        public string Name { get; set; } 
        public bool? Status { get; set; }
    }
}
