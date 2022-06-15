﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.ActivitiesEntity
{
    public class ActivitiesEntityInsertModel
    {
        [JsonPropertyName("competition_activity_id")]
        public int CompetitionActivityId { get; set; }
        public string Name { get; set; }

        //IMG file convert base 64 string
        [JsonPropertyName("base64_string")]
        public string Base64String { get; set; }
        //---------Author to check user is Leader of Club and Collaborate in Copetition       
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
    }
}
