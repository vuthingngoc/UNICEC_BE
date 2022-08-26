using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class UserRequestModel : PagingRequest
    {
        [FromQuery(Name = "universityId")] 
        public int? UniversityId { get; set; }
        [FromQuery(Name = "departmentId")]
        public int? DepartmentId { get; set; }
        [FromQuery(Name = "searchString")]
        public string SearchString { get; set; }
        [FromQuery(Name = "status")]
        public UserStatus? Status { get; set; }
    }
}
