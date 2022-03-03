using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNICS.Business.Common
{
    public class PagingRequest
    {
        // maxPageSize chỉ cho 50 thôi
        const int maxPageSize = 50;
        //luôn luôn ở trang 1
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
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
