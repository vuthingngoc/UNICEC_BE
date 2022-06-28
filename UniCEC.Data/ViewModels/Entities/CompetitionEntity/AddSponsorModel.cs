﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.CompetitionEntity
{
    internal class AddSponsorModel
    {
        public string Name { get; set; }
        [JsonPropertyName("base64_string_img")]
        public string Base64StringImg { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }
    }
}
