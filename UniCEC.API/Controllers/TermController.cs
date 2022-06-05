using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.TermSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/term")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class TermController : ControllerBase
    {
        private ITermService _itermService;
        public TermController(ITermService termService)
        {
            _itermService = termService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get term corresponding in a club")]
        public async Task<IActionResult> GetTermById(int id, [FromQuery, BindRequired] int clubId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewTerm term = await _itermService.GetById(token, clubId, id);
                return Ok(term);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("club/{id}")]
        [SwaggerOperation(Summary = "Get current term in a club")]
        public async Task<IActionResult> GetCurrentTermByClub(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewTerm term = await _itermService.GetCurrentTermByClub(token, id);
                return Ok(term);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("club/{id}/search")]
        [SwaggerOperation(Summary = "Searching term")]
        public async Task<IActionResult> GetTermByConditions(int id, [FromQuery] TermRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewTerm> terms = await _itermService.GetByConditions(token, id, request);
                return Ok(terms);
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
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add new term in a club")]
        public async Task<IActionResult> InsertTerm([FromBody] TermInsertModel term)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewTerm viewTerm = await _itermService.Insert(token, term);
                return Ok(viewTerm);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update term")]
        public async Task<IActionResult> UpdateTerm([FromBody] TermUpdateModel term, [FromQuery, BindRequired] int clubId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _itermService.Update(token, term, clubId);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        //[HttpDelete("{id}")]
        //[SwaggerOperation(Summary = "Delete term")]
        //public async Task<IActionResult> DeleteTerm(int id)
        //{
        //    try
        //    {
        //        await _itermService.Delete(id);
        //        return NoContent();
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    };
        //}
    }
}
