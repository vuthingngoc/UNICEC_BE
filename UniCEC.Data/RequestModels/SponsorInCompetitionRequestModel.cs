using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class SponsorInCompetitionRequestModel : PagingRequest
    {
        //University Id
        [FromQuery(Name = "universityId")]
        public int? UnversityId { get; set; }

        //Club Id
        [FromQuery(Name = "clubId")]
        public int? ClubId { get; set; }
       
        //Status
        public SponsorInCompetitionStatus? status { get; set; }
    }
}
