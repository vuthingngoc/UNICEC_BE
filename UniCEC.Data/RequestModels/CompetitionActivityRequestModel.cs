using Microsoft.AspNetCore.Mvc;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class CompetitionActivityRequestModel : PagingRequest
    {

        [FromQuery(Name = "universityId")]
        public int UniversityId { get; set; }

        [FromQuery(Name = "clubId")]
        public int? ClubId { get; set; }
        [FromQuery(Name = "status")]
        public CompetitionActivityStatus? Status { get; set; }
        [FromQuery(Name = "seedsPoint")]
        public int? SeedsPoint { get; set; }
        [FromQuery(Name = "numberOfMember")]
        public int? NumberOfMember { get; set; }
        //[FromQuery(Name = "begin-time")]
        //public DateTime? BeginTime { get; set; }
        //[FromQuery(Name = "end-time")]
        //public DateTime? EndTime { get; set; }
        //[FromQuery(Name = "create-time")]
        //public DateTime? CreateTime { get; set; }

    }
}
