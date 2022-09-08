using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.MajorSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/majors")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class MajorController : ControllerBase
    {
        IMajorService _majorService;

        public MajorController(IMajorService majorService)
        {
            _majorService = majorService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get major by id - Authenticated user")]
        public async Task<IActionResult> GetMajorById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewMajor major = await _majorService.GetById(token, id);
                return Ok(major);
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
        [SwaggerOperation(Summary = "Get majors by conditions - Authenticated user")]
        public async Task<IActionResult> GetMajorsByConditions([FromQuery] MajorRequestModel request)
        {
            try 
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewMajor> majors = await _majorService.GetByConditions(token, request);
                return Ok(majors);
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
        [SwaggerOperation(Summary = "Get majors by competition id - Authenticated user")]
        public async Task<IActionResult> GetMajorsByCompetition(int id, [FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewMajor> majors = await _majorService.GetByCompetition(id, request);
                return Ok(majors);
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
        [SwaggerOperation(Summary = "Insert new major - System admin")]
        public async Task<IActionResult> InsertMajor([BindRequired] string name)
        {
            try 
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewMajor major = await _majorService.Insert(token, name);
                return Ok(major);
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
        [SwaggerOperation(Summary = "Update a major - System admin")]
        public async Task<IActionResult> UpdateMajor([FromBody] MajorUpdateModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _majorService.Update(token, model);
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
        [SwaggerOperation(Summary = "Delete major by id - System admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _majorService.Delete(token, id);
                return NoContent();
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
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
        }       
    }
}
