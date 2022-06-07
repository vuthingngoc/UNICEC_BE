using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniCEC.Business.Services.ClubRoleSvc;

namespace UniCEC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubRoleController : ControllerBase
    {
        private IClubRoleService _clubRoleService;

        public ClubRoleController(IClubRoleService clubRoleService)
        {
            _clubRoleService = clubRoleService;
        }

        

    }
}
