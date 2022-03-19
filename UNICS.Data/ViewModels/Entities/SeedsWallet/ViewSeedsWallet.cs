using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.SeedsWallet
{
    public class ViewSeedsWallet
    {
        public string Id { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        public double Ammount { get; set; }
    }
}
