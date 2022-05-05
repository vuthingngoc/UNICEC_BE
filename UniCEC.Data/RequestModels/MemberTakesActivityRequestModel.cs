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
    public class MemberTakesActivityRequestModel : PagingRequest
    {
        //Lấy list tasks của Id member đó
        [FromQuery(Name = "memberId")]
        public int MemberId { get; set; }
        //lấy các task của Id member đó theo clubId mà nó tham gia
        [FromQuery(Name = "clubId")]
        public int? ClubId { get; set; }
        //search by Status của task thuộc về member đó 
        [FromQuery(Name = "status")]
        public MemberTakesActivityStatus? Status { get; set; }

    }
}
