using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class CompetitionUnAuthorizeRequestModel : PagingRequest
    {
        public bool? Sponsor { get; set; }
        public string? Name { get; set; }
    }
}
