using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.ViewModels.Common;

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
        public Task<IActionResult> GetAllUser(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpGet("search")]
        public Task<IActionResult> GetUserByCondition(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetAllUser(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetAllUser(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetAllUser(PagingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
