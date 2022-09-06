using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCEC.Data.RequestModels
{
    public class MatchRequestModel
    {
        public int RoundId { get; set; }
        public int MatchTypeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Scores { get; set; }
        public int Status { get; set; }
    }
}
