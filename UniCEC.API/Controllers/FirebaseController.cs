using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.FirebaseSvc;
using UniCEC.Data.ViewModels.Firebase.Auth;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/firebase")]
    [ApiController]
    [ApiVersion("1.0")]
    public class FirebaseController : ControllerBase
    {

        private IFirebaseService _firebaseService;

        public FirebaseController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // POST api/<FirebaseController>
        [HttpPost]
        public async Task<IActionResult> AuthenticateUser()
        {
            try
            {
                //Get The IDToken From FE
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return BadRequest();                

                string token = header["Authorization"].ToString().Split(" ")[1];
                string deviceId = (Request.Headers)["X-Device-Token"].ToString();
                ViewUserInfo userInfo = await _firebaseService.Authentication(token, deviceId);
                return (userInfo != null) ? Ok(userInfo) : BadRequest("Please login with your university email");                
            }
            catch(UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }


}
