using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class TeamRequestModel : PagingRequest
    {
        //Competition Id
        [FromQuery(Name = "competitionId")]
        public int CompetitionId { get; set; }      
     
    }
}
