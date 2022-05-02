using Microsoft.AspNetCore.Mvc;

namespace UniCEC.Data.ViewModels.Common
{
    public class PagingRequest
    {
        const int MAX_PAGE_SIZE = 50;
        private int _pageSize = 10;
        // change name of parameter
        [FromQuery(Name = "currentPage")]
        public int CurrentPage { get; set; } = 1;         
        [FromQuery(Name = "pageSize")]
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
            }
        }
    }
}
