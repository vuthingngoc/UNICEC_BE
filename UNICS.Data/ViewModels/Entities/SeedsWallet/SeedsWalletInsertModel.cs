using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.SeedsWallet
{
    public class SeedsWalletInsertModel
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        public double Ammount { get; set; }
    }
}
