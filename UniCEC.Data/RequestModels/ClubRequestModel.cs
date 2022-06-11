using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class ClubRequestModel : PagingRequest
    {
        [FromQuery(Name = "universityId"), BindRequired]
        public int UniversityId { get; set; }
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
    }
}
