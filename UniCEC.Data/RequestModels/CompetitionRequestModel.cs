using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class CompetitionRequestModel : PagingRequest
    {
        //Club Id
        [FromQuery(Name = "clubId")]
        public int? ClubId { get; set; }
        //Search Event
        [FromQuery(Name = "event")]
        public bool? Event { get; set; }
        //Scope
        [FromQuery(Name = "scope")]
        public CompetitionScopeStatus? Scope { get; set; }
        //Name
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
        //Status
        [FromQuery(Name = "statuses")]
        public List<CompetitionStatus>? Statuses { get; set; }
    }
}
