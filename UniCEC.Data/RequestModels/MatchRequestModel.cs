using System;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class MatchRequestModel : PagingRequest
    {
        public int RoundId { get; set; }
        public int MatchTypeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Scores { get; set; }
        public int Status { get; set; }
    }
}
