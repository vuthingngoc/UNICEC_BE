using System;

namespace UNICS.Data.ViewModels.Entities.Blog
{
    public class ViewBlog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? Status { get; set; }
        public int? UserId { get; set; }
        public int CompetitionId { get; set; }
    }
}
