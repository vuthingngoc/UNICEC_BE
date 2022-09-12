using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.ParticipantSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Participant;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/participants")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ParticipantController : ControllerBase
    {
        private IParticipantService _participantService;

        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        // POST api/<ParticipantController>
        [Authorize(Roles = "Student")]
        [HttpPost()]
        [SwaggerOperation(Summary = "Add paritcipant in Competition Or Event")]
        public async Task<IActionResult> Insert([FromBody] ParticipantInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewParticipant viewParticipant = await _participantService.Insert(model, token);
                if (viewParticipant != null)
                {

                    return Ok(viewParticipant);
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

        // PUT api/<ParticipantController>
        [Authorize(Roles = "Student")]
        [HttpPut("attendance")]
        [SwaggerOperation(Summary = "Update Attendance of paritcipant in Competition Or Event")]
        public async Task<IActionResult> UpdateAttendance([FromQuery(Name = "seedsCode"), BindRequired] string seedsCode)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                bool viewParticipant = await _participantService.UpdateAttendance(seedsCode, token);
                if (viewParticipant)
                {

                    return Ok();
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

        // PUT api/<ParticipantController>
        [Authorize(Roles = "Student")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get Status a Paritcipant in Competition Or Event")]
        public async Task<IActionResult> Get([FromQuery(Name = "competitionId"), BindRequired] int competitionId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewParticipant viewParticipant = await _participantService.GetByCompetitionId(competitionId, token);
                return Ok(viewParticipant);
            }
            catch (NullReferenceException)
            {
                return Ok(new object());
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
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [Authorize(Roles = "Student")]
        [HttpGet("list-participant")]
        [SwaggerOperation(Summary = "Get Status a Paritcipant in Competition Or Event")]
        public async Task<IActionResult> GetByConditions([FromQuery] ParticipantRequestModel request)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                PagingResult<ViewParticipant> result = await _participantService.GetByConditions(request, token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
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
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

    }
}
