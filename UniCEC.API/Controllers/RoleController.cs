using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.RoleSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Role;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/role")]
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
        [HttpGet("roles")]
        [SwaggerOperation(Summary = "Get list roles")]
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
                    //Not has data
                    return Ok(new List<object>());
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
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get role by Id")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                ViewRole result = await _roleService.GetByRoleId(id);
                if (result == null)
                {
                    //Not has data
                    return Ok(new object());
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(Summary = "Insert role")]
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

        
    }
}
