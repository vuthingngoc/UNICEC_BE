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
                ViewTerm term = await _itermService.GetById(clubId, id);
                return Ok(term);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            throw new NotImplementedException();
        }

        [HttpGet("club/{id}")]
        [SwaggerOperation(Summary = "Get all terms in a club")]
        public async Task<IActionResult> GetTermByClub(int id, [FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewTerm> terms = await _itermService.GetByClub(id, request);
                return Ok(terms);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
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
                PagingResult<ViewTerm> terms = await _itermService.GetByConditions(id, request);
                return Ok(terms);
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
        [SwaggerOperation(Summary = "Add term")]
        public async Task<IActionResult> InsertTerm(TermInsertModel term)
        {
            try
            {
                ViewTerm viewTerm = await _itermService.Insert(term);
                return Created($"api/v1/term/{viewTerm.Id}", viewTerm);
            }
            catch(ArgumentNullException ex)
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
        public async Task<IActionResult> UpdateTerm(TermUpdateModel term)
        {
            try
            {
                await _itermService.Update(term);
                return Ok();
            }
            catch (ArgumentNullException ex)
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

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete term")]
        public async Task<IActionResult> DeleteTerm(int id)
        {
            try
            {
                await _itermService.Delete(id);
                return NoContent();
            }
            catch (ArgumentNullException ex)
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
            };
        }
    }
}
