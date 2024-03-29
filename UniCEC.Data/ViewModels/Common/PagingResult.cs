﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UniCEC.Data.ViewModels.Common
{
    public class PagingResult<T>
    {
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }        
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
        //Front End
        [JsonPropertyName("has_previous")]
        public bool HasPrevious => CurrentPage > 1;
        [JsonPropertyName("has_next")]
        public bool HasNext => CurrentPage < TotalPages;

        public PagingResult(List<T> items, int count, int currentPage, int pageSize)
        {
            int size = items.Count;
            TotalCount = count;
            PageSize = (size < pageSize) ? size : pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.Items = items;
        }
    }
}
