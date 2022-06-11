using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Influencer
{
    public class InfluencerInsertModel
    {
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
       
    }
}
