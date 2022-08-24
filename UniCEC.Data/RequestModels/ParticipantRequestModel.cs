using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class ParticipantRequestModel : PagingRequest
    {
        //Competition Id
        [FromQuery(Name = "competitionId"), BindRequired]
        public int CompetitionId { get; set; }

        [FromQuery(Name = "hasTeam")]
        public bool? HasTeam { get; set; }

        [FromQuery(Name = "hasAttendance")]
        public bool? HasAttendance { get; set; }

        //Club Id
        [FromQuery(Name = "clubId"), BindRequired]
        public int ClubId { get; set; }
    }
}
