using System;
using System.Text.Json.Serialization;

namespace UNICS.Data.ViewModels.Entities.Blog
{
    public class BlogUpdateModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
    }
}
