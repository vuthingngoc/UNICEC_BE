using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/notifications")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class NotificationController : ControllerBase
    {

    }
}
