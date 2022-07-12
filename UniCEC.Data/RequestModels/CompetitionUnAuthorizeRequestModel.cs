using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class CompetitionUnAuthorizeRequestModel : PagingRequest
    {
        [FromQuery(Name = "sponsor")]
        public bool? Sponsor { get; set; }

        [FromQuery(Name = "name")]
        public string? Name { get; set; }

        [FromQuery(Name = "nearlyDate")]
        public bool? NearlyDate { get; set; }

        [FromQuery(Name = "mostView")]
        public bool? MostView { get; set; }

    }
}
