using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCEC.Data.Enum
{
    public enum CompetitionStatus
    {       
        Register,
        UpComing, 
        StartCompetition,
        OnGoing,
        EndCompetition,
        Publish,                    //--> đã chốt chỉ cho Update DateTime
        Draft,
        PendingReview,
        Approve,       
        Pending,
        Complete, 
        Evaluation,
        Closed,
        Canceling
    }
}
