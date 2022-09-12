using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class CompetitionUnAuthorizeRequestModel : PagingRequest
    {
        [FromQuery(Name = "sponsor")]
        public bool? Sponsor { get; set; }

        [FromQuery(Name = "name")]
        public string Name { get; set; }

        [FromQuery(Name = "nearlyDate")]
        public bool? NearlyDate { get; set; }

        [FromQuery(Name = "mostView")]
        public bool? MostView { get; set; }
    }
}
