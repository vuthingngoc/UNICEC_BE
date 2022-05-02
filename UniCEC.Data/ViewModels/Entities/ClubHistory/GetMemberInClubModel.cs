using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.ClubHistory
{
    public class GetMemberInClubModel
    {
        [FromQuery(Name = "user-id")]
        public int UserId
        {
            get; set;
        }
        [FromQuery(Name = "club-id")]
        public int ClubId
        {
            get; set;
        }
        [FromQuery(Name = "term-id")]
        public int TermId
        {
            get; set;
        }
    }
}
