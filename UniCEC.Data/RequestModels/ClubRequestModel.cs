using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class ClubRequestModel : PagingRequest
    {
        public string Name { get; set; }
        public bool? Status { get; set; }
    }
}
