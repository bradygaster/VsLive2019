using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VsLiveSanDiego.Services;

namespace VsLiveSanDiego.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private JwtBearerTokenService JwtBearerTokenService;

        public AuthenticateController(JwtBearerTokenService jwtBearerTokenService)
        {
            JwtBearerTokenService = jwtBearerTokenService;
        }

        [AllowAnonymous]
        public IActionResult Authenticate([FromQuery] string username)
        {
            var token = JwtBearerTokenService.Authenticate(username);
            return Ok(token);
        }
    }
}
