using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.ActivitiesEntity
{
    public class AddActivitiesEntity
    {
        //Activities Entity
        [JsonPropertyName("name_entity")]
        public string NameEntity { get; set; }

        [JsonPropertyName("base64_string_entity")]
        public string Base64StringEntity { get; set; }
    }
}
