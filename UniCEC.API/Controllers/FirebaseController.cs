using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.UserSvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class FirebaseController : ControllerBase
    {
        private IUserService _userService;
        public FirebaseController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<FirebaseController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FirebaseController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FirebaseController>
        [HttpPost]
        public async Task<IActionResult> AuthenticateUser()
        {
            try
            {

            }
            catch (Exception e)
            {
                BadRequest(e.Message);
            }

            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync("idToken");
            string uid = decodedToken.Uid;
            //decodedToken

            return Ok();
        }



        // PUT api/<FirebaseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FirebaseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
