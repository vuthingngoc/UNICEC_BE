using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionRoleSvc;
using UniCEC.Data.ViewModels.Entities.CompetitionRole;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/competition-roles")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "System Admin")]
    public class CompetitionRoleController : ControllerBase
    {
        private ICompetitionRoleService _competitionRoleService;
        public CompetitionRoleController(ICompetitionRoleService competitionRoleService)
        {
            _competitionRoleService = competitionRoleService;
        }

        //// GET: api/<CompetitionRoleController>
        //Get List Roles
        [HttpGet()]
        [SwaggerOperation(Summary = "Get all competition roles - System admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                List<ViewCompetitionRole> result = await _competitionRoleService.GetAll();
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

        // GET api/<CompetitionRoleController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Competition Role by Id")]
        public async Task<IActionResult> GetCompetitionRoleById(int id)
        {
            try
            {
                ViewCompetitionRole result = await _competitionRoleService.GetByCompetitionRoleId(id);
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

        // POST api/<CompetitionRoleController>
        [HttpPost]
        [SwaggerOperation(Summary = "Insert competition role")]
        public async Task<IActionResult> InsertCompetitionRole([FromBody] CompetitionRoleInsertModel model)
        {
            try
            {
                ViewCompetitionRole result = await _competitionRoleService.Insert(model);
                return Ok(result);

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

        // PUT api/<CompetitionRoleController>/5
        [HttpPut]
        [SwaggerOperation(Summary = "Update competition role")]
        public async Task<IActionResult> UpdateCompetitionRole([FromBody] ViewCompetitionRole model)
        {
            try
            {
                await _competitionRoleService.Update(model);
                return Ok();

            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
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

        
    }
}
