using System;
using System.Collections.Generic;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class BlogType
    {
        public BlogType()
        {
            Blogs = new HashSet<Blog>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
