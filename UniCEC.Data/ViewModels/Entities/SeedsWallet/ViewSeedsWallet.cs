using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.SeedsWallet
{
    public class ViewSeedsWallet
    {
        public string Id { get; set; }
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }
        public double Amount { get; set; }
        public bool Status { get; set; }
    }
}
