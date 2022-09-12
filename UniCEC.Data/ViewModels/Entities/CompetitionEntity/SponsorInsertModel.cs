using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.ViewModels.Entities.CompetitionEntity
{
    public class SponsorInsertModel : CompetitionEntityInsertModel
    {
        public List<AddSponsorModel> Sponsors { get; set; }
    }
}
