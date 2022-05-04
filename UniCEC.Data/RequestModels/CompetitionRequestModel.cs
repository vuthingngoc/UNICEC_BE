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
    public class CompetitionRequestModel : PagingRequest
    {
        //Club Id
        [FromQuery(Name = "club-id")]
        public int ClubId { get; set; }
        //Serach Event
        [FromQuery(Name = "search_event")]
        public bool? Event { get; set; }
        //Public
        public bool? Public { get; set; }   
        //Status
        public CompetitionStatus? Status { get; set; }
    }
}
