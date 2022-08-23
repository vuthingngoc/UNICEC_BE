using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class AdminUniGetCompetitionRequestModel : PagingRequest
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }

        //Get Entities
        [FromQuery(Name = "getEntities")]
        public bool? getEntities { get; set; }
    }
}
