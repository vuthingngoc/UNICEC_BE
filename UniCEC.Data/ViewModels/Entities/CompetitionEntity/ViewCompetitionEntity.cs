﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.CompetitionEntity
{
    public class ViewCompetitionEntity
    {
        [JsonPropertyName("competition_entity_id")]
        public int Id { get; set; }

        [JsonPropertyName("competition_id")]
        public int CompetitionId { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
    }
}
