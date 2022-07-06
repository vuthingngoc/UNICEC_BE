using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Entities.SeedsWallet
{
    public class SeedsWalletUpdateModel
    {
        [BindRequired]
        public int Id { get; set; }
        [JsonPropertyName("student_id"), BindRequired]
        public int StudentId { get; set; }
        public double? Amount { get; set; }
        public bool? Status { get; set; }
    }
}
