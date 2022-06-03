using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Influencer
{
    public class InfluencerInsertModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
       
    }
}
