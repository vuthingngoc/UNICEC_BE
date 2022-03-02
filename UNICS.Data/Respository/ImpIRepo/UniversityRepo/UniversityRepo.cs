
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Data.Models.DB;
using UNICS.Data.Respository.GenericRepo;

namespace UNICS.Data.Respository.ImpIRepo.UniversityRepo
{
    public class UniversityRepo : Repository<University>, IUniversityRepo
    {
        public UniversityRepo(UNICSContext context) : base(context)
        {

        }


    }
}

