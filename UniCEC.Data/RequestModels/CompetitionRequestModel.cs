﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [FromQuery(Name = "clubId")]
        public int? ClubId { get; set; }
        //Serach Event
        [FromQuery(Name = "event")]
        public bool? Event { get; set; }
        //Public
        [FromQuery(Name = "scope")]
        public CompetitionScopeStatus? Scope { get; set; }
        //Status
        [FromQuery(Name = "statuses")]
        public List<CompetitionStatus>? Statuses { get; set; }
    }
}
