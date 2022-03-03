using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Data.ViewModels.University;

namespace UNICS.Business.Services.UniversitySvc
{
    public interface IUniversityService
    {
        public ViewUniversity GetUniversityById(string id);
    }
}
