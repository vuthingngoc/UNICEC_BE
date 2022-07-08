using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.ClubRoleSvc;
using UniCEC.Data.ViewModels.Entities.ClubRole;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/club-roles")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]

    public class ClubRoleController : ControllerBase
    {
        private IClubRoleService _clubRoleService;

        public ClubRoleController(IClubRoleService clubRoleService)
        {
            _clubRoleService = clubRoleService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get club role by id - Authenticated user")]
        public async Task<IActionResult> GetClubRoleById(int id)
        {
            try
            {
                ViewClubRole clubRole = await _clubRoleService.GetById(id);
                return Ok(clubRole);
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

        [HttpGet]
        [SwaggerOperation(Summary = "Get all club role - Authenticated user")]
        public async Task<IActionResult> GetAllClubRole()
        {
            try
            {
                List<ViewClubRole> clubRoles = await _clubRoleService.GetAll();
                return Ok(clubRoles);
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
        [SwaggerOperation(Summary = "Insert club role - System admin")]
        public async Task<IActionResult> InsertNewClubRole([FromQuery] string name)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewClubRole clubRole = await _clubRoleService.Insert(name, token);
                return Ok(clubRole);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
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
        [SwaggerOperation(Summary = "Update club role - System admin")]
        public async Task<IActionResult> UpdateClubRole([FromBody, BindRequired] ViewClubRole model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _clubRoleService.Update(model, token);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
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
        [SwaggerOperation(Summary = "Delete club role - System admin")]
        public async Task<IActionResult> DeleteClubRole(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _clubRoleService.Delete(id, token);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
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
