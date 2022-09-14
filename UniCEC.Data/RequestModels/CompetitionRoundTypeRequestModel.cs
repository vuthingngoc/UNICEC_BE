using Microsoft.AspNetCore.Mvc;

namespace UniCEC.Data.RequestModels
{
    public class CompetitionRoundTypeRequestModel
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
    }
}
