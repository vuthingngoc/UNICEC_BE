using Microsoft.AspNetCore.Mvc;

namespace UniCEC.Data.ViewModels.Entities.Member
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
    }
}
