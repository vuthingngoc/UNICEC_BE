using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.DepartmentSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/departments")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            try
            {
                ViewDepartment department = await _departmentService.GetById(id);
                return Ok(department);
            }
            catch(NullReferenceException)
            {
                return Ok(new object());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetDepartmentByConditions([FromQuery] DepartmentRequestModel request)
        {
            try 
            {
                PagingResult<ViewDepartment> departments = await _departmentService.GetByConditions(request);
                return Ok(departments);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch(NullReferenceException)
            {
                return Ok(new List<object>());
            }
        }

        [HttpGet("competition/{id}")]
        public async Task<IActionResult> GetDepartmentByCompetition(int id, [FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewDepartment> departments = await _departmentService.GetByCompetition(id, request);
                return Ok(departments);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
        }

        [HttpPost]
        [Authorize(Roles = "System Admin")]
        public async Task<IActionResult> InsertDepartment(string name)
        {
            try 
            {
                ViewDepartment viewDepartment = await _departmentService.Insert(name);
                return Ok(viewDepartment);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "System Admin")]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentUpdateModel department)
        {
            try
            {
                await _departmentService.Update(department);
                return Ok(); 
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "System Admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                await _departmentService.Delete(id);
                return NoContent();
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
        }       
    }
}
