using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.RoleSvc;
using UniCEC.Data.ViewModels.Entities.Role;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/roles")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "System Admin")]
    public class RoleController : ControllerBase
    {
        IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        //Get List Roles
        [HttpGet()]
        [SwaggerOperation(Summary = "Get all roles - System admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                List<ViewRole> result = await _roleService.GetAll();
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

        // GET api/<RoleController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get role by Id")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                ViewRole result = await _roleService.GetByRoleId(id);
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

        //InsertRoleModel
        [HttpPost]
        [SwaggerOperation(Summary = "Insert role")]
        public async Task<IActionResult> InsertRoleId(string roleName)
        {
            try
            {
                ViewRole result = await _roleService.Insert(roleName);
                return Ok(result);
                
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

        // PUT api/<RoleController>/5
        [HttpPut]
        [SwaggerOperation(Summary = "Update role")]
        public async Task<IActionResult> UpdateRole([FromBody] ViewRole model)
        {
            try
            {
                await _roleService.Update(model);
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

        [HttpDelete]
        [SwaggerOperation(Summary = "Delete role - System admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _roleService.Delete(id);
                return Ok();

            }
            catch (NullReferenceException ex)
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
