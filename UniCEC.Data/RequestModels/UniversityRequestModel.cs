using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.RequestModels
{
    public class UniversityRequestModel : PagingRequest
    {
        // find by name
        public string Name { get; set; }
    }
}
