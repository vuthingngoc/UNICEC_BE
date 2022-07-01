using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Enum;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class AdminUniUpdateCompetitionStatusModel
    {
        public int Id { get; set; }

        public CompetitionStatus? Status { get; set; }
    }
}
