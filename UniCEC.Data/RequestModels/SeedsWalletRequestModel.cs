using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class SeedsWalletRequestModel : PagingRequest
    {
        [FromQuery(Name = "universityId")]
        public int? UniversityId { get; set; }
        [FromQuery(Name = "studentId")]
        public int? StudentId { get; set; }
        [FromQuery(Name = "amount")]
        public double? Amount { get; set; }
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
    }
}
