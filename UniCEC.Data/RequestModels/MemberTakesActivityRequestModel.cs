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
    public class MemberTakesActivityRequestModel : PagingRequest
    {
      
        [FromQuery(Name = "competitionActivityId")]
        public int CompetitionActivityId { get; set; }
        
        [FromQuery(Name = "status")]
        public MemberTakesActivityStatus? Status { get; set; }

        //Check
        [FromQuery(Name = "clubId")]
        public int ClubId { get; set; }

    }
}
