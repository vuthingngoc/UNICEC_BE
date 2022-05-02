using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class TermRequestModel : PagingRequest
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "createYear")]
        public string CreateYear { get; set; }
        [FromQuery(Name = "endYear")]
        public string EndYear { get; set; }
    }
}
