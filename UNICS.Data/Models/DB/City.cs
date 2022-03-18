﻿using System;
using System.Collections.Generic;

#nullable disable

namespace UNICS.Data.Models.DB
{
    public partial class City
    {
        public City()
        {
            Universities = new HashSet<University>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<University> Universities { get; set; }
    }
}
