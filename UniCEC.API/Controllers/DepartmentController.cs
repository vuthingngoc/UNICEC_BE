using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/departments")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]

    public class DepartmentController : ControllerBase
    {
        private IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService majorService)
        {
            _departmentService = majorService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get department by id - authenticated user")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewDepartment department = await _departmentService.GetById(token, id);
                return Ok(department);
            }
            catch (NullReferenceException)
            {
                return Ok(new object());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("code/{id}")]
        [SwaggerOperation(Summary = "Get department by code - authenticated user")]
        public async Task<IActionResult> GetDepartmentByCode([FromRoute(Name = "id")] string code)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewDepartment department = await _departmentService.GetByCode(token, code);
                return Ok(department);
            }
            catch (NullReferenceException)
            {
                return Ok(new object());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Get majors by conditions - authenticated user")]
        public async Task<IActionResult> GetDepartmentsByConditions([FromQuery] DepartmentRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewDepartment> departments = await _departmentService.GetByConditions(token, request);
                return Ok(departments);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Insert new department - University admin")]
        public async Task<IActionResult> InsertDepartment([FromBody] DepartmentInsertModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewDepartment viewDepartment = await _departmentService.Insert(token, model);
                return Ok(viewDepartment);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update a major - University admin")]
        public async Task<IActionResult> UpdateDepartment([FromBody] ViewDepartment department)
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
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete major by id - University admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _departmentService.Delete(token, id);                
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }            
        }
    }
}
