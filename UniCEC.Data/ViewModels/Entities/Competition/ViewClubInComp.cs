using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.Competition
{
    public class ViewClubInComp
    {
        public int Id { get; set; }
        [JsonPropertyName("club_id")]
        public int ClubId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }       
        public string Fanpage { get; set;}
        [JsonPropertyName("is_owner")]
        public bool IsOwner { get; set; }   
    }
}
