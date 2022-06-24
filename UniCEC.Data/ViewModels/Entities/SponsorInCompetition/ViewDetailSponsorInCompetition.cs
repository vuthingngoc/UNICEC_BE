using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.SponsorInCompetition
{
    public class ViewDetailSponsorInCompetition : ViewSponsorInCompetition
    {
        public string Comment { get; set; }

        [JsonPropertyName("review_date")]
        public DateTime? ReviewDate { get; set; }
        public string Feedback { get; set; }
      
        
    }
}
