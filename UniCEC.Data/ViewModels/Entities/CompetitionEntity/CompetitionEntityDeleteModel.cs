using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.ViewModels.Entities.CompetitionEntity
{
    public class CompetitionEntityDeleteModel : CompetitionEntityInsertModel
    {
        [JsonPropertyName("competition_entity_id")]
        public int CompetitionEntityId;
    }
}
