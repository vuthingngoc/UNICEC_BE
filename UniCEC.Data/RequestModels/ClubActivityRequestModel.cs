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

        [FromQuery(Name = "universityId")]
        public int UniversityId { get; set; }

        [FromQuery(Name = "clubId")]
        public int? ClubId { get; set; }
        [FromQuery(Name = "status")]
        public ClubActivityStatus? Status { get; set; }
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
