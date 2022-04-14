using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser([FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewUser> users = await _userService.GetAllPaging(request);
                return Ok(users);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetUserByCondition([FromQuery] UserRequestModel request)
        {
            try
            {
                PagingResult<ViewUser> users = await _userService.GetUserCondition(request);
                return Ok(users);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }


        [HttpPost]
        public async Task<IActionResult> InsertUser([FromBody] UserInsertModel request)
        {
            try
            {
                ViewUser user = await _userService.Insert(request);
                return Created($"api/v1/[controller]/{user.Id}", user);
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
        public async Task<IActionResult> UpdateUser([FromBody] ViewUser request)
        {
            try
            {
                bool result = await _userService.Update(request);
                return (result) ? Ok() : StatusCode(500, "Internal Server Exception");
            }
            catch(NullReferenceException ex)
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
