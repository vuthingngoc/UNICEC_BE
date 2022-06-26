using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class MajorRequestModel : PagingRequest
    {
        public string Name { get; set; }
        public bool? Status { get; set; }
    }
}
