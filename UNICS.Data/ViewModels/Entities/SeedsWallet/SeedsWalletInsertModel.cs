using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.SeedsWallet
{
    public class SeedsWalletInsertModel
    {
        public double? Ammount { get; set; }
        [JsonPropertyName("student_id")]
        public int? StudentId { get; set; }
    }
}
