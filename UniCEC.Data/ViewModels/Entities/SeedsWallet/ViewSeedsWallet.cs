using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.SeedsWallet
{
    public class ViewSeedsWallet
    {
        public string Id { get; set; }
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }
        public double Ammount { get; set; }
    }
}
