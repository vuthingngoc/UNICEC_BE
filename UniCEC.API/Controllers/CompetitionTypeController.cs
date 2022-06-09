using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionTypeSvc;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/competition-types")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CompetitionTypeController : ControllerBase
    {
        private ICompetitionTypeService _competitionTypeService;

        public CompetitionTypeController(ICompetitionTypeService competitionTypeService)
        {
            _competitionTypeService = competitionTypeService;
        }

        [HttpGet("competition-types")]
        [SwaggerOperation(Summary = "Get list competition types")]
        public async Task<IActionResult> GetCompetitionTypes([FromQuery] PagingRequest request )
        {
            try
            {
                PagingResult<ViewCompetitionType> result = await _competitionTypeService.GetAllPaging(request);           
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


        // GET api/<CompetitionTypeController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get competition type by id")]
        public async Task<IActionResult> GetCompetitionTypeById(int id)
        {
            try
            {
                ViewCompetitionType result = await _competitionTypeService.GetByCompetitionTypeId(id);
                
                
                    //Not has data
                    return Ok(new object());
                
                
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

        // POST api/<CompetitionTypeController>
        [HttpPost]
        [SwaggerOperation(Summary = "Insert competition type")]
        public async Task<IActionResult> Insert([FromBody] CompetitionTypeInsertModel model)
        {
            try
            {
                ViewCompetitionType result = await _competitionTypeService.Insert(model);
                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
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

        // PUT api/<CompetitionTypeController>/5
        [HttpPut]
        [SwaggerOperation(Summary = "Update competition type")]
        public async Task<IActionResult> Update([FromBody] ViewCompetitionType model)
        {
            try
            {
                Boolean check = false;
                check = await _competitionTypeService.Update(model);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
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
