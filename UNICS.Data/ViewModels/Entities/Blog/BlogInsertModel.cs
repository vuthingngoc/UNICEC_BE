using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNICS.Data.ViewModels.Entities.Blog
{
    public class BlogInsertModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? Status { get; set; }
        public int? UserId { get; set; }
        public int CompetitionId { get; set; }
    }
}
