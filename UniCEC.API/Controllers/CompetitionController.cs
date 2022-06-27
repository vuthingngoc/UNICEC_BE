using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionSvc;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.Influencer;
using UniCEC.Data.ViewModels.Entities.InfluencerInComeptition;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/competitions")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CompetitionController : ControllerBase
    {
        private ICompetitionService _competitionService;

        public CompetitionController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }

        // GET: api/<CompetitionController>
        [HttpGet]
        [SwaggerOperation(Summary = "Get EVENT or COMPETITION by conditions, 0.Launching, 1.Registering, 2.HappenningSoon, 3.Happening, 4.Ending, 5.Canceling")]
        public async Task<IActionResult> GetCompOrEve([FromQuery] CompetitionRequestModel request)
        {
            try
            {
                PagingResult<ViewCompetition> result = await _competitionService.GetCompOrEve(request);
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

        // GET api/<CompetitionController>/5
        //[Authorize(Roles = "Student")]
        //[HttpGet("view-all-apply")]
        //[SwaggerOperation(Summary = "Get ALL sponsor apllies in ONE competition or event")]
        //public async Task<IActionResult> GetViewAllApplyInCompOrEve([FromQuery] SponsorApplyRequestModel request)
        //{
        //    try
        //    {
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        PagingResult<ViewSponsorInCompetition> result = await _competitionService.GetViewAllApplyInCompOrEve(request, token);
        //        return Ok(result);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        return Ok(new List<object>());
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}


        // GET api/<CompetitionController>/5
        //[Authorize(Roles = "Student")]
        //[HttpGet("view-detail-apply")]
        //[SwaggerOperation(Summary = "Get Detail sponsor aplly in ONE competition or event")]
        //public async Task<IActionResult> GetViewDetailApplyInCompOrEve([FromQuery(Name = "sponsorInCompetitionId")] int SponsorInCompetitionId, [FromQuery(Name = "clubId")] int ClubId)
        //{
        //    try
        //    {
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        ViewDetailSponsorInCompetition result = await _competitionService.GetViewDetailApplyInCompOrEve(SponsorInCompetitionId, ClubId, token);
        //        return Ok(result);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        return Ok(new object());
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}


        // GET: api/<CompetitionController>
        [HttpGet("top3")]
        [SwaggerOperation(Summary = "Get top 3 EVENT or COMPETITION by club, status, public")]
        public async Task<IActionResult> GetTop3CompOrEve([FromQuery(Name = "clubId")] int? ClubId, [FromQuery(Name = "event")] bool? Event, [FromQuery(Name = "status")] CompetitionStatus? status, [FromQuery(Name = "scope")] CompetitionScopeStatus? Scope)
        {
            try
            {
                List<ViewCompetition> result = await _competitionService.GetTop3CompOrEve(ClubId, Event, status, Scope);
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


        // GET api/<CompetitionController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get detail of EVENT or COMPETITON by id")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                ViewDetailCompetition result = await _competitionService.GetById(id);

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

        //ClubLeader
        // POST api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpPost()]
        [SwaggerOperation(Summary = "Insert EVENT or COMPETITON, if Event please put value at number-of-group = 0 ")]
        //phải có author student
        public async Task<IActionResult> Insert([FromBody] LeaderInsertCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];


                ViewDetailCompetition viewCompetition = await _competitionService.LeaderInsert(model, token);
                if (viewCompetition != null)
                {

                    return Ok(viewCompetition);
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
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpPut()]
        [SwaggerOperation(Summary = "Update detail EVENT or COMPETITON")]
        public async Task<IActionResult> Update([FromBody] LeaderUpdateCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.LeaderUpdate(model, token);
                if (check)
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
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpPut("test-be")]
        [SwaggerOperation(Summary = "Update detail EVENT or COMPETITON")]
        public async Task<IActionResult> Update_BE([FromBody] LeaderUpdateCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.UpdateBE(model, token);
                if (check)
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
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        // PUT api/<CompetitionController>/5
        //[Authorize(Roles = "Student")]
        //[HttpPut("feedback")]
        //[SwaggerOperation(Summary = "Feedback sponsor apply in Competition or Event  Status:  1.Approved, 2.Rejected")]
        //public async Task<IActionResult> FeedbackSponsorApply([FromBody] FeedbackSponsorInCompetitionModel model)
        //{
        //    try
        //    {
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];
        //        Boolean check = false;
        //        check = await _competitionService.FeedbackSponsorApply(model, token);
        //        if (check)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}



        // DELETE api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpDelete()]
        [SwaggerOperation(Summary = "Canceling EVENT or COMPETITION")]
        public async Task<IActionResult> Delete([FromBody] LeaderDeleteCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.LeaderDelete(model, token);
                if (check)
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
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        // DELETE api/<CompetitionController>/5
        //[Authorize(Roles = "Student")]
        //[HttpDelete("remove-sponsor")]
        //[SwaggerOperation(Summary = "Club manager delete sponsor in competition or event")]
        //public async Task<IActionResult> LeaderDeleteSponsorInCompetition([FromBody] SponsorInCompetitionDeleteModel model)
        //{
        //    try
        //    {
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];
        //        Boolean check = false;
        //        check = await _competitionService.LeaderDeleteSponsorInCompetition(model, token);
        //        if (check)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}



        //---------------------------------------------------------------------------Competition In Department
        //POST api/<CompetitionController>

        //[Authorize(Roles = "Student")]
        //[HttpPost("department")]
        //[SwaggerOperation(Summary = "Add department for competition")]
        //public async Task<IActionResult> AddCompetitionInDepartment([FromBody] CompetitionInMajorInsertModel model)
        //{
        //    try
        //    {
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        List<ViewCompetitionInMajor> result = await _competitionService.AddCompetitionInDepartment(model, token);

        //        if (result != null)
        //        {
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}


        //---------------------------------------------------------------------------Influencer In Competition
        //POST api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpPost("influencer")]
        [SwaggerOperation(Summary = "Add influencer in competition")]
        public async Task<IActionResult> AddInfluencerInCompetition([FromBody] InfluencerInComeptitionInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                List<ViewInfluencerInCompetition> result = await _competitionService.AddInfluencerInCompetition(model, token);

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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        // DELETE api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpDelete("influencer")]
        [SwaggerOperation(Summary = "Delete influencer in Competition")]
        public async Task<IActionResult> DeleteInluencerInCompetition([FromBody] InfluencerInCompetitionDeleteModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.DeleteInluencerInCompetition(model, token);
                if (check)
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
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        //---------------------------------------------------------------------------Competition In Club
        //POST api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpPost("club")]
        [SwaggerOperation(Summary = "Add another club in competition")]
        public async Task<IActionResult> AddClubCollaborate([FromBody] CompetitionInClubInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewCompetitionInClub result = await _competitionService.AddClubCollaborate(model, token);
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        //---------------------------------------------------------------------------Sponsor in Competition

        //// GET api/<CompetitionController>/5
        //[Authorize(Roles = "Sponsor")]
        //[HttpGet("sponsor-view-apply")]
        //[SwaggerOperation(Summary = "Sponsor view all applies")]
        //public async Task<IActionResult> GetSponsorViewAllApplyInCompOrEve([FromQuery]SponsorInCompetitionRequestModel request)
        //{
        //    try
        //    {
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        PagingResult<ViewSponsorInCompetition> result = await _competitionService.GetSponsorViewAllApplyInCompOrEve(request,token);
        //        return Ok(result);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        return Ok(new List<object>());
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}

        //// GET api/<CompetitionController>/5
        //[Authorize(Roles = "Sponsor")]
        //[HttpGet("sponsor-view-detail-apply")]
        //[SwaggerOperation(Summary = "Sponsor view detail apply")]
        //public async Task<IActionResult> GetSponsorViewDetailApplyInCompOrEve([FromQuery(Name = "sponsorInCompetitionId")] int SponsorInCompetitionId)
        //{
        //    try
        //    {
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        ViewDetailSponsorInCompetition result = await _competitionService.GetSponsorViewDetailApplyInCompOrEve(SponsorInCompetitionId, token);
        //        return Ok(result);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        return Ok(new object());
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}


        ////POST api/<SponsorInCompetitionController>
        //[Authorize(Roles = "Sponsor")]
        //[HttpPost("sponsor-apply")]
        //[SwaggerOperation(Summary = "Sponsor apply in competition")]
        //public async Task<IActionResult> AddSponsorCollaborate([FromBody] SponsorInCompetitionInsertModel model)
        //{
        //    try
        //    {
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        ViewDetailSponsorInCompetition result = await _competitionService.AddSponsorCollaborate(model, token);
        //        if (result != null)
        //        {

        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}


        //// DELETE api/<CompetitionController>/5
        //[Authorize(Roles = "Sponsor")]
        //[HttpDelete("sponsor-deny")]
        //[SwaggerOperation(Summary = "Sponsor deny in Competition")]
        //public async Task<IActionResult> SponsorDeny([FromBody] SponsorInCompetitionDenyModel model)
        //{
        //    try
        //    {
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];
        //        Boolean check = false;
        //        check = await _competitionService.SponsorDenyInCompetition(model, token);
        //        if (check)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}


    }
}
