using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class UserRequestModel : PagingRequest
    {
        [FromQuery(Name = "university-id")] 
        public int? UniversityId { get; set; }
        [FromQuery(Name = "major-id")]
        public int? MajorId { get; set; }
        [FromQuery(Name = "fullname")]
        public string Fullname { get; set; }
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
    }
}
