using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Common
{
    public class PagingResult<T>
    {
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }
        //Front End
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        //
        public List<T> Items { get; set; }
        //

        public PagingResult(List<T> items, int count, int currentPage, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.Items = items;
        }
    }
}
