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
    public class ClubActivityRequestModel : PagingRequest
    {

        [FromQuery(Name = "university-id")]
        public int UniversityId { get; set; }

        [FromQuery(Name = "club-id")]
        public int? ClubId { get; set; }

        public ClubActivityStatus? Status { get; set; }
        [FromQuery(Name = "seeds-point")]
        public int? SeedsPoint { get; set; }
        [FromQuery(Name = "number-of-member")]
        public int? NumberOfMember { get; set; }
        //[FromQuery(Name = "begin-time")]
        //public DateTime? BeginTime { get; set; }
        //[FromQuery(Name = "end-time")]
        //public DateTime? EndTime { get; set; }
        //[FromQuery(Name = "create-time")]
        //public DateTime? CreateTime { get; set; }


    }
}
