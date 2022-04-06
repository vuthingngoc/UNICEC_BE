using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.MajorSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;

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



        [HttpGet("{id}")]
        public async Task<IActionResult> GetMajorById(int id)
        {
            try
            {
                ViewMajor major = await _majorService.GetByMajorId(id);
                return Ok(major);
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

        [HttpGet]
        public async Task<IActionResult> GetMajorByCondition(MajorRequestModel request)
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
        public async Task<IActionResult> Insert([FromBody] MajorInsertModel major)
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
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(ViewMajor major)
        {
            try
            {
                bool result = await _majorService.Update(major);
                if(result) return Ok();
                return StatusCode(500, "Internal Server Exception");

            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool result = await _majorService.Delete(id);
                if (!result)
                {
                    return StatusCode(500, "Internal Server Exception");
                }
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }

            return NoContent();
        }
    }
}
