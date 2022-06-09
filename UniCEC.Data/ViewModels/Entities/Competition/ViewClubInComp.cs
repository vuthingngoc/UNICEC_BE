using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewClubInComp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }       
        public string Fanpage { get; set;}
        [JsonPropertyName("is_owner")]
        public bool IsOwner { get; set; }   
    }
}
