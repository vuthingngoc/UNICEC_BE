using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInDeparmentRepo
{
    public interface ICompetitionInDepartmentRepo : IRepository<CompetitionInDepartment> 
    {
        public Task<List<ViewDeparmentInComp>> GetListDepartment_In_Competition(int CompetitionId);

        public Task<List<int>> GetListDepartmentId_In_Competition(int CompetitionId);
    }
}
