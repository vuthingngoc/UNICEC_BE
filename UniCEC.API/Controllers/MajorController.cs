using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.MajorSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/majors")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]

    public class MajorController : ControllerBase
    {
        private IMajorService _majorService;

        public MajorController(IMajorService majorService)
        {
            _majorService = majorService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllMajor([FromQuery] PagingRequest request)
        //{
        //    try
        //    {
        //        PagingResult<ViewMajor> majors = await _majorService.GetAllPaging(request);
        //        return Ok(majors);
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //}

        //[HttpGet("university/{id}")]
        //public async Task<IActionResult> GetMajorByUniversity(int id, [FromQuery] PagingRequest request)
        //{
        //    try
        //    {
        //        PagingResult<ViewMajor> majors = await _majorService.GetByUniversity(id, request);
        //        return Ok(majors);
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //    catch(NullReferenceException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //}

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get major by id - authenticated user")]
        public async Task<IActionResult> GetMajorById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewMajor major = await _majorService.GetById(token, id);
                return Ok(major);
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
        [SwaggerOperation(Summary = "Get major by code - authenticated user")]
        public async Task<IActionResult> GetMajorByCode([FromRoute(Name = "id")] string code)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewMajor major = await _majorService.GetByCode(token, code);
                return Ok(major);
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
        public async Task<IActionResult> GetMajorByConditions([FromQuery] MajorRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewMajor> majors = await _majorService.GetMajorByConditions(token, request);
                return Ok(majors);
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
        [SwaggerOperation(Summary = "Insert new major - University admin")]
        public async Task<IActionResult> InsertMajor([FromBody] MajorInsertModel major)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewMajor viewMajor = await _majorService.Insert(token, major);
                return Ok(viewMajor);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
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
        public async Task<IActionResult> UpdateMajor([FromBody] ViewMajor major)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _majorService.Update(token, major);
                return Ok();                
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
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
        public async Task<IActionResult> DeleteMajor(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _majorService.Delete(token, id);                
                return NoContent();
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
