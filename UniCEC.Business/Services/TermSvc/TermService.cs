using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.TermRepo;

namespace UniCEC.Business.Services.TermSvc
{
    public class TermService : ITermService
    {
        private ITermRepo _termRepo;
        public TermService(ITermRepo termRepo)
        {
            _termRepo = termRepo;
        }


    }
}
