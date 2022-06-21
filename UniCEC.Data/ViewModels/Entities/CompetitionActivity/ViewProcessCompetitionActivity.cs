using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.CompetitionActivity
{
    public class ViewProcessCompetitionActivity 
    {
        
        public int Id { get; set; }

        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        [JsonPropertyName("competition_name")]
        public string CompetitionName { get; set; }

        [JsonPropertyName("process_status")]
        public CompetitionActivityProcessStatus ProcessStatus { get; set; }

        public CompetitionActivityStatus Status { get; set; }

        //[JsonPropertyName("create_time")]
        //public DateTime CreateTime { get; set; }
        //public DateTime Ending { get; set; }
        //public string Name { get; set; }
        //public PriorityStatus Priority { get; set; }

        //Extra infomation to view process
        //[JsonPropertyName("num_of_member_join")]
        //public int NumOfMemberJoin { get; set; }

        //[JsonPropertyName("num_member_doing_task")]
        //public int NumMemberDoingTask { get; set; }

        //[JsonPropertyName("num_member_done_task")]
        //public int NumMemberDoneTask { get; set; }

        //[JsonPropertyName("num_member_done_late_task")]
        //public int NumMemberDoneLateTask { get; set; }

        //[JsonPropertyName("num_member_late_task")]
        //public int NumMemberLateTask { get; set; }

    }
}
