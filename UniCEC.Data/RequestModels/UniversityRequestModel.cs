using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class UniversityRequestModel : PagingRequest
    {
        // find-by-name
        public string? Name { get; set; }
        // find-by-cityId
        [FromQuery(Name = "city-id")]
        public int? CityId { get; set; }
        //find-by-status
        public bool? Status { get; set; }
    }
}
