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
    [Route("api/v1/university")]
    [ApiController]
    [ApiVersion("1.0")]
   
    public class UniversityController : ControllerBase
    {
        // GET: api/<UniversityController>

        //tạo service
        private IUniversityService _universityService;

        //constructor để DI Service vào
        public UniversityController(IUniversityService universityService)
        {
            this._universityService = universityService;
        }


        [HttpGet("universities")]
        [SwaggerOperation(Summary = "Get universities by conditions")]
        public async Task<IActionResult> GetUniversityByConditions([FromQuery] UniversityRequestModel request)
        {
            try
            {
                PagingResult<ViewUniversity> result = await _universityService.GetUniversitiesByConditions(request);
                
                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    //Not has data
                    return Ok(new List<object>());
                }
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }

        //Get 1 university by ID
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get university by id")]
        public async Task<IActionResult> GetUniversityById(int id)
        {
            try {
                
                ViewUniversity result = await _universityService.GetUniversityById(id);
                if (result == null)
                {
                    //Not has data
                    return Ok(new object());
                }
                else 
                {
                    //
                    return Ok(result);
                }
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // POST api/<UniversityController>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(Summary = "Insert university - Admin")]
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
            catch (ArgumentNullException ex)
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
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [SwaggerOperation(Summary = "Update university - Admin")]
        public async Task<IActionResult> UpdateUniversityById([FromBody] ViewUniversity university)
        {
            try {
                Boolean check = false;
                check = await _universityService.Update(university);
                if (check)
                {
                    return Ok();
                }
                else { 
                   return BadRequest();
                }
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

        // DELETE api/<UniversityController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete university - Admin")]
        public async Task<IActionResult> DeleteUniversityById(int id)
        {
            try
            {
                bool result = false;
                result = await _universityService.Delete(id);
                if (result)
                {
                    return Ok();
                }
                else {
                    return BadRequest();
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
               return StatusCode(500, "Internal server exception");
            }
        }
    }
}
