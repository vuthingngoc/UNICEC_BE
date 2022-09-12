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
    public class GetStudentJoinCompOrEve : PagingRequest
    {
        //Search Event
        [FromQuery(Name = "event")]
        public bool? Event { get; set; }
        //Scope
        [FromQuery(Name = "scope")]
        public CompetitionScopeStatus? Scope { get; set; }
        //Name
        [FromQuery(Name = "name")]
        public string Name { get; set; }
    }
}
