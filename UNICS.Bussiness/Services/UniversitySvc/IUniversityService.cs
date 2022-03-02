using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Bussiness.Common;
using UNICS.Bussiness.ViewModels.University;

namespace UNICS.Bussiness.Services.UniversitySvc
{
     public interface IUniversityService
    {
        // get university by id
        public ViewUniversity GetUniversityById(String id);

    }
}
