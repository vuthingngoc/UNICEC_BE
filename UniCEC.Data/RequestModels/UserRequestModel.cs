using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class UserRequestModel : PagingRequest
    {
        [FromQuery(Name = "universityId")] 
        public int? UniversityId { get; set; }
        [FromQuery(Name = "majorId")]
        public int? DepartmentId { get; set; }
        [FromQuery(Name = "fullName")]
        public string Fullname { get; set; }
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
    }
}
