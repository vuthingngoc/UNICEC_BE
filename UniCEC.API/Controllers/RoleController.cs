using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.RoleSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Role;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class RoleController : ControllerBase
    {
        IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }



        //Get List Roles
        [HttpGet("GetListRoles")]
        public async Task<IActionResult> GetRoles([FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewRole> result = await _roleService.GetAllPaging(request);

                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // GET api/<RoleController>/5
        [HttpGet("GetRoleBy{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                ViewRole result = await _roleService.GetByRoleId(id);
                if (result == null)
                {
                    return NotFound("Not Found This Role in System");
                }
                else
                {
                    //
                    return Ok(result);
                }
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }

        }

        //InsertRoleModel
        [HttpPost("InsertRole")]
        public async Task<IActionResult> InsertRoleId([FromBody] RoleInsertModel model)
        {
            try
            {
                ViewRole result = await _roleService.Insert(model);
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

        // PUT api/<RoleController>/5
        [HttpPut("UpdateUniversityById")]
        public async Task<IActionResult> UpdateRole ([FromBody] ViewRole model)
        {
            try
            {
                Boolean check = false;
                check = await _roleService.Update(model);
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

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
