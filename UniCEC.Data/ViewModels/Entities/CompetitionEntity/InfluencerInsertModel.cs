using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.ViewModels.Entities.CompetitionEntity
{
    public class InfluencerInsertModel : CompetitionEntityInsertModel
    {
        public List<AddInfluencerModel> Influencers { get; set; }
    }
}
