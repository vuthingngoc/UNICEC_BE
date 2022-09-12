using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class UniversityRequestModel : PagingRequest
    {
        // find-by-name
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        // find-by-cityId
        [FromQuery(Name = "cityId")]
        public int? CityId { get; set; }
        //find-by-status
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
       
    }
}
