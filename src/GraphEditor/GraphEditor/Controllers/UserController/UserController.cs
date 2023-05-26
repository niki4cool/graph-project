using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GraphEditor.Auth;
using GraphEditor.Model;
using System.Text.Json;

namespace GraphEditor.Controllers.UserController
{

    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserStore<User> userStore;

        private readonly UserRecordValidator userValidator;
        private readonly ILookupNormalizer keyNormalizer;

        private readonly JwtGenerator jwtSignInHandler;

        public UserController(IUserStore<User> userStore,
                              ILookupNormalizer keyNormalizer,
                              UserRecordValidator userValidator,
                              JwtGenerator jwtSignInHandler)
        {
            this.userStore = userStore;
            this.keyNormalizer = keyNormalizer;
            this.userValidator = userValidator;
            this.jwtSignInHandler = jwtSignInHandler;
        }

        [HttpPut("Register")]
        public async Task<ActionResult> Create([FromBody] CreateUserRequest request)
        {
            var normalized = keyNormalizer.NormalizeName(request.UserName);
            var user = await userStore.FindByNameAsync(normalized, CancellationToken.None);
            if (user != null)
                return BadRequest("Username already exists");

            var result = await userValidator.CreateAsync(request.UserName, request.Password);
            if (result == null || !result.Succeeded)
                return BadRequest("Unable to create new user");
            return Created(Request.Path, result);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteUserRequest request)
        {
            var message = JsonSerializer.Serialize(new { Error = "Not a valid user" });
            var user = await userStore.FindByIdAsync(request.Id, CancellationToken.None);
            if (user == null)
                return NotFound(message);
            if (request.UserName != user.Name)
                return BadRequest(message);
            var isValidUser = await userValidator.IsValidUser(request.UserName, request.Password);
            if (!isValidUser)
                return BadRequest(message);
            await userStore.DeleteAsync(user, CancellationToken.None);
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var message = JsonSerializer.Serialize(new { Error = "Not a valid user" });
            var isValidUser = await userValidator.IsValidUser(request.UserName, request.Password);
            if (!isValidUser)
                return BadRequest(message);

            var normalized = keyNormalizer.NormalizeName(request.UserName);
            var user = await userStore.FindByNameAsync(normalized, CancellationToken.None);
            if (user == null)
                return NotFound(message);

            var identity = IdentityHelper.CreateFullIdentity(user);
            var jwt = jwtSignInHandler.Generate(new ClaimsPrincipal(identity)).ToString();
            
            return Ok(new LoginResponse()
            {
                Id = user.Id,
                Name = user.Name,
                Token = jwt,
            });
        }
    }

    public static class IdentityHelper
    {
        public static ClaimsIdentity CreateFullIdentity(User user)
        {
            var identity = new ClaimsIdentity();

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));

            foreach (var graph in user.CanEdit)
                identity.AddClaim(new Claim(ClaimTypes.Role, graph.EditRole));
            foreach (var graph in user.CanView)
                identity.AddClaim(new Claim(ClaimTypes.Role, graph.ViewRole));
            foreach (var graph in user.Creations)
                identity.AddClaim(new Claim(ClaimTypes.Role, graph.EditRole));
            return identity;
        }
    }
}
