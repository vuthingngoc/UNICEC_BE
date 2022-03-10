using Microsoft.AspNetCore.Mvc;

namespace UNICS.Data.ViewModels.Common
{
    public class PagingRequest
    {
        // maxPageSize chỉ cho 50 thôi
        const int MAX_PAGE_SIZE = 50;
        //
        // luôn luôn ở trang 1
        // change name of parameter
        [FromQuery(Name = "current-page")]
        public int currentPage { get; set; } = 1;
        //cho mặc định có 10 records trong 1 trang
        private int _pageSize = 10;
        [FromQuery(Name = "page-size")]
        public int pageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                // nếu true thì trả ra maxPageSize chỉ cho 50 thôi
                // còn false thì sẽ lấy giá trị ng truyền
                _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
            }
        }
    }
}
