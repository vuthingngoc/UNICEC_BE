using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.UniversitySvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.University;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/universities")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]

    public class UniversityController : ControllerBase
    {
        // GET: api/<UniversityController>

        //tạo service
        private IUniversityService _universityService;

        //constructor để DI Service vào
        public UniversityController(IUniversityService universityService)
        {
            _universityService = universityService;
        }


        [HttpGet("search")]
        [SwaggerOperation(Summary = "Get universities by conditions - All roles")]
        public async Task<IActionResult> GetUniversityByConditions([FromQuery] UniversityRequestModel request)
        {
            try
            {
                PagingResult<ViewUniversity> result = await _universityService.GetUniversitiesByConditions(request);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all universities - All roles")]
        public async Task<IActionResult> GetUniversity()
        {
            try
            {
                List<ViewUniversity> result = await _universityService.GetUniversities();
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }

        //Get 1 university by ID
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get university by id - All roles")]
        public async Task<IActionResult> GetUniversityById(int id)
        {
            try
            {
                ViewUniversity result = await _universityService.GetUniversityById(id);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new object());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // POST api/<UniversityController>
        [Authorize(Roles = "System Admin")]
        [HttpPost]
        [SwaggerOperation(Summary = "Insert university - System Admin")]
        public async Task<IActionResult> InsertUniversity([FromBody] UniversityInsertModel model)
        {
            try
            {
                //gọi service
                ViewUniversity result = await _universityService.Insert(model);
                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }

        // PUT api/<UniversityController>/5
        //[Authorize(Roles = "Admin")]
        [HttpPut]
        [SwaggerOperation(Summary = "Update university - The university admin and system Admin")]
        public async Task<IActionResult> UpdateUniversityById([FromBody] UniversityUpdateModel university)
        {
            try
            {
                Boolean check = false;
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                check = await _universityService.Update(university, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

    }
}
