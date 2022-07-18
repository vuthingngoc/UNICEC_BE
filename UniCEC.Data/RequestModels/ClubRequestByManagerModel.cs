using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class ClubRequestByManagerModel : PagingRequest
    {
        [FromQuery(Name = "clubId"), BindRequired]
        public int clubId { get; set; }
        [FromQuery(Name = "universityIds")]
        public List<int> UniversityIds { get; set; }
        [FromQuery(Name = "name")]
        public string Name { get; set; }        
    }
}
