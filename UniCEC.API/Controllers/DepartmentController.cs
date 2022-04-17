using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.DepartmentSvc;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DepartmentController : ControllerBase
    {
        IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public Task<IActionResult> GetAllDepartment(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpGet("competition/{id}")]
        public Task<IActionResult> GetDepartmentByCompetition(int competitionId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<IActionResult> InsertDepartment(DepartmentInsertModel department)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<IActionResult> UpdateDepartment(DepartmentInsertModel department)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Task<IActionResult> DeleteDepartment(DepartmentInsertModel department)
        {
            throw new NotImplementedException();
        }

    }
}
