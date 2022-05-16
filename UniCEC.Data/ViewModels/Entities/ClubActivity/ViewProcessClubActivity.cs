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

        [JsonPropertyName("num_member_doing_task")]
        public int NumMemberDoingTask { get; set; }

        [JsonPropertyName("num_member_done_task")]
        public int NumMemberDoneTask { get; set; }

        [JsonPropertyName("num_member_done_late_task")]
        public int NumMemberDoneLateTask { get; set; }

        [JsonPropertyName("num_member_late_task")]
        public int NumMemberLateTask { get; set; }

        [JsonPropertyName("num_member_out_task")]
        public int NumMemberOutTask { get; set; }
    }
}
