using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNICS.Data.ViewModels.Entities.Campus
{
    public class ViewCampus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public string Address { get; set; }
        public int? UniversityId { get; set; }
        public int? AreaId { get; set; }
    }
}
