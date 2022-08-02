using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.MemberTakesActivity
{
    public class ViewMemberTakesActivity
    {
        public int Id { get; set; }
        [JsonPropertyName("competition_activity_id")]
        public int CompetitionActivityId { get; set; }
        [JsonPropertyName("member_id")]
        public int MemberId { get; set; }

        [JsonPropertyName("member_name")]
        public string MemberName { get; set; }

        [JsonPropertyName("member_img")]
        public string MemberImg { get; set; }

        [JsonPropertyName("booker_id")]
        public int BookerId { get; set; }

        [JsonPropertyName("booker_name")]
        public string BookerName { get; set; }

    }
}
