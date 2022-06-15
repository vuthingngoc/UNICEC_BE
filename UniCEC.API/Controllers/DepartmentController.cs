using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(Summary = "Get department by id - Authenticated user")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewDepartment department = await _departmentService.GetById(token, id);
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
        [SwaggerOperation(Summary = "Get departments by conditions - Authenticated user")]
        public async Task<IActionResult> GetDepartmentByConditions([FromQuery] DepartmentRequestModel request)
        {
            try 
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewDepartment> departments = await _departmentService.GetByConditions(token, request);
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
        [SwaggerOperation(Summary = "Get departments by competition id - Authenticated user")]
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
        [SwaggerOperation(Summary = "Insert new department - System admin")]
        public async Task<IActionResult> InsertDepartment([BindRequired] string name)
        {
            try 
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewDepartment viewDepartment = await _departmentService.Insert(token, name);
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
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "System Admin")]
        [SwaggerOperation(Summary = "Update a department - System admin")]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentUpdateModel department)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _departmentService.Update(token, department);
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
            catch (ArgumentException ex)
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
        [SwaggerOperation(Summary = "Delete department by id - System admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _departmentService.Delete(token, id);
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
