using Microsoft.AspNetCore.Mvc;
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
    public class TeamRequestModel : PagingRequest
    {
        //Competition Id
        [FromQuery(Name = "competitionId"), BindRequired]
        public int CompetitionId { get; set; }

        [FromQuery(Name = "teamName")]
        public string? TeamName { get; set; }

        [FromQuery(Name = "status")]
        public TeamStatus? status { get; set; }

    }
}
