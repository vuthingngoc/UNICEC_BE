using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.Enum;
using UniCEC.Data.JWT;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get user by id")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewUser users = await _userService.GetById(token, id);
                return Ok(users);
            }
            catch (NullReferenceException)
            {
                return Ok(new object());
            }
            catch (SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search user - University admin and system admin")]
        [Authorize(Roles = "University Admin, System Admin")]
        public async Task<IActionResult> GetUserByCondition([FromQuery] UserRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewUser> users = await _userService.GetUserCondition(token, request);
                return Ok(users);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        [HttpPost("grant-account")]
        [SwaggerOperation(Summary = "Grant account for university admin - System admin")]
        [Authorize(Roles = "System Admin")]
        public async Task<IActionResult> InsertUser([FromBody] UserAccountInsertModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                bool result = await _userService.Insert(token, request);
                return (result) ? Ok() : StatusCode(500, "Internal Server Exception");
            }
            catch (ArgumentException ex)
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]        
        [SwaggerOperation(Summary = "Log in account for university admin - University admin")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccount([FromBody] UserLoginModel model)
        {
            try
            {
                string token = await _userService.LoginAccount(model);
                return Ok(token);                
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("logout")]
        [SwaggerOperation(Summary = "Log out account - Authenticated user")]
        public async Task<IActionResult> LogoutAccount()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                int userId = _userService.DecodeToken(token, "Id");
                await _userService.UpdateStatusOnline(userId, false);
                return Ok();
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }
        
        [Authorize(Roles = "Student")]
        [HttpPut("token")]
        [SwaggerOperation(Summary = "Update university of student - student who haven't choose a university yet")]
        public async Task<IActionResult> UpdateUserWithJWT(UserUpdateWithJWTModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _userService.UpdateInfoToken(model, token);
                UserTokenModel user = await _userService.GetUserTokenById(model.Id, token);
                string userToken = JWTUserToken.GenerateJWTTokenUser(user);
                return Ok(userToken);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
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
        [SwaggerOperation(Summary = "Update user - authenticated user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                bool result = await _userService.Update(request, token);
                return (result) ? Ok() : StatusCode(500, "Internal Server Exception");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (ArgumentException ex)
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

        [HttpPut("{id}/status")]
        [Authorize(Roles = "System Admin")]
        [SwaggerOperation(Summary = "Update status user - System admin")]
        public async Task<IActionResult> UpdateStatusUser(int id, UserStatus status)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _userService.UpdateStatusUser(id, status, token);
                return Ok();                
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (ArgumentException ex)
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

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete user - System Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                bool result = await _userService.Delete(token, id);
                return (result) ? NoContent() : StatusCode(500, "Internal Server Exception");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
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
