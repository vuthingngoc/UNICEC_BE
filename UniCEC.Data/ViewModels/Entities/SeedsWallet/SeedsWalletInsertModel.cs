using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.SeedsWallet
{
    public class SeedsWalletInsertModel
    {
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }
        public double Ammount { get; set; }
    }
}
