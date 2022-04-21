using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.MajorSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;
using System.Collections.Generic;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MajorController : ControllerBase
    {
        private IMajorService _majorService;

        public MajorController(IMajorService majorService)
        {
            _majorService = majorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMajor([FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewMajor> majors = await _majorService.GetAllPaging(request);
                return Ok(majors);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("university/{id}")]
        public async Task<IActionResult> GetMajorByUniversity(int id, [FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewMajor> majors = await _majorService.GetByUniversity(id, request);
                return Ok(majors);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetMajorByCondition([FromQuery] MajorRequestModel request)
        {
            try
            {
                PagingResult<ViewMajor> majors = await _majorService.GetMajorByCondition(request);
                return Ok(majors);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertMajor([FromBody] MajorInsertModel major)
        {
            try
            {
                ViewMajor viewMajor = await _majorService.Insert(major);
                return Created($"api/v1/[controller]/{viewMajor.Id}", viewMajor);
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
        public async Task<IActionResult> UpdateMajor([FromBody] ViewMajor major)
        {
            try
            {
                bool result = await _majorService.Update(major);
                return (result) ? Ok() : StatusCode(500, "Internal Server Exception");                
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
        public async Task<IActionResult> DeleteMajor(int id)
        {
            try
            {
                bool result = await _majorService.Delete(id);                
                return (result) ? NoContent() : StatusCode(500, "Internal Server Exception");
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
