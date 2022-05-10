using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.ClubActivity
{
    public class ViewProcessClubActivity : ViewClubActivity
    {
        //Extra infomation 
        [JsonPropertyName("num_of_member_join")]
        public int NumOfMemberJoin { get; set; }

        [JsonPropertyName("member_task_status")]
        public int NumOfMemberTakesTaskStatus { get; set; }
    }
}
