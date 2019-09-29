using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VsLiveSanDiego.Services;

namespace VsLiveSanDiego.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private JwtBearerTokenService JwtBearerTokenService;

        public AuthenticateController(
            JwtBearerTokenService jwtBearerTokenService,
            UserList userList,
            ILogger<AuthenticateController> logger)
        {
            JwtBearerTokenService = jwtBearerTokenService;
            UserList = userList;
            Logger = logger;
        }

        public UserList UserList { get; }
        public ILogger<AuthenticateController> Logger { get; }

        [AllowAnonymous]
        public IActionResult Authenticate([FromQuery] string username)
        {
            if(UserList.CurrentUsernames.Contains(username))
            {
                Logger.LogWarning($"User {username} is already logged in.");
                return BadRequest();
            }

            UserList.CurrentUsernames.Add(username);
            Logger.LogInformation($"User {username} logged in.");
            var token = JwtBearerTokenService.Authenticate(username);
            return Ok(token);
        }
    }
}
