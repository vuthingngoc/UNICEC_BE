using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.MajorSvc;
using UniCEC.Business.Services.RoleSvc;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.Enum;
using UniCEC.Data.JWT;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;
using UniCEC.Data.ViewModels.Entities.Role;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IRoleService _roleService;
        private IMajorService _majorService;

        public UserController(IUserService userService, IRoleService roleService, IMajorService majorService)
        {
            _userService = userService;
            _roleService = roleService;
            _majorService = majorService;
        }

        [HttpGet("university/{id}")]
        [SwaggerOperation(Summary = "Get users by universityId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersByUniversity(int id, UserStatus status, [FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewUser> users = await _userService.GetByUniversity(id, status, request);
                return Ok(users);
            }
            catch (NullReferenceException ex)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByCondition([FromQuery] UserRequestModel request)
        {
            try
            {
                PagingResult<ViewUser> users = await _userService.GetUserCondition(request);
                return Ok(users);
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


        [HttpPost]
        [SwaggerOperation(Summary = "Insert user")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertUser([FromBody] UserInsertModel request)
        {
            try
            {
                ViewUser user = await _userService.Insert(request);
                return Created($"api/v1/[controller]/{user.Id}", user);
                //get RoleName
                //ViewRole role = await _roleService.GetByRoleId(user.RoleId);
                //string roleName = role.RoleName;
                //string clientTokenUser = JWTUserToken.GenerateJWTTokenStudent(user, roleName);
                //return Ok(clientTokenUser);

            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update user")]
        public async Task<IActionResult> UpdateUser([FromBody] ViewUser request)
        {
            try
            {
                bool result = await _userService.Update(request);
                return (result) ? Ok() : StatusCode(500, "Internal Server Exception");
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPut("{id}/logout")]
        [SwaggerOperation(Summary = "Log out account")]
        public async Task<IActionResult> LogoutAccount(int id)
        {
            try
            {
                await _userService.UpdateStatusOnline(id, false);
                return Ok();
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

        //-------------------LOGIN
        [Authorize(Roles = "Student")]
        [HttpPut("jwttoken")]
        [SwaggerOperation(Summary = "Update infomations student")]
        public async Task<IActionResult> UpdateUserWithJWT([FromBody] ViewUser request)
        {
            try
            {
                bool result = await _userService.Update(request);
                if (result)
                {
                    //get ViewUser
                    ViewUser user = await _userService.GetUserByUserCode(request.UserCode);
                    //get RoleName
                    ViewRole role = await _roleService.GetByRoleId(user.RoleId);
                    string roleName = role.RoleName;
                    //get MajorName
                    ViewMajor major = await _majorService.GetMajorById((int)request.MajorId);
                    string majorName = major.Name;
                    string clientTokenUser = JWTUserToken.GenerateJWTTokenStudent(user, roleName, majorName);
                    return Ok(clientTokenUser);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }



        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                bool result = await _userService.Delete(id);
                return (result) ? NoContent() : StatusCode(500, "Internal Server Exception");
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }
    }
}
