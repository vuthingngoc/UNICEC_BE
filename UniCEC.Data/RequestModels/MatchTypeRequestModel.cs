using Microsoft.AspNetCore.Mvc;

namespace UniCEC.Data.RequestModels
{
    public class MatchTypeRequestModel
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
    }
}
