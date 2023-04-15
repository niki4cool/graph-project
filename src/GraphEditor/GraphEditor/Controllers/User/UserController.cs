using GraphEditor.Hubs;
using GraphEditor.Models.Auth.User;
using GraphEditor.Models.CRUD;
using GraphEditor.Models.Graph;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using GraphEditor.Models.Auth.Handlers;

namespace GraphEditor.Controllers.User
{

    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserStore<UserRecord> userStore;
        private readonly IRepository<GraphRecord> graphRepository;

        private readonly UserRecordValidator userValidator;
        private readonly ILookupNormalizer keyNormalizer;

        private readonly JwtSignInHandler jwtSignInHandler;

        public UserController(IUserStore<UserRecord> userStore,
                              IRepository<GraphRecord> graphRepository,
                              ILookupNormalizer keyNormalizer,
                              UserRecordValidator userValidator,
                              JwtSignInHandler jwtSignInHandler)
        {
            this.userStore = userStore;
            this.keyNormalizer = keyNormalizer;
            this.userValidator = userValidator;
            this.jwtSignInHandler = jwtSignInHandler;
            this.graphRepository = graphRepository;
        }

        [HttpPut("Register")]
        public async Task<ActionResult> Create([FromBody] CreateUserRequest request)
        {
            var normalized = keyNormalizer.NormalizeName(request.UserName);
            var user = await userStore.FindByNameAsync(normalized, CancellationToken.None);
            if (user != null)
                return base.BadRequest("Username already exists");

            var result = await userValidator.CreateAsync(request.UserName, request.Email, request.Password);
            if (result == null || !result.Succeeded)
                return base.BadRequest("Unable to create new user");
            return await GetByName(request.UserName);
        }

        [HttpGet("name/{userName}")]
        public async Task<ActionResult> GetByName(string userName)
        {
            var normalized = keyNormalizer.NormalizeName(userName);
            var user = await userStore.FindByNameAsync(normalized, CancellationToken.None);
            if (user == null)
                return base.NotFound();
            return Ok(new GetUserResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            });
        }

        [HttpGet("id/{Id}")]
        public async Task<ActionResult> GetById(string Id)
        {
            var user = await userStore.FindByIdAsync(Id, CancellationToken.None);
            if (user == null)
                return base.NotFound();
            return Ok(new GetUserResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            });
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(string Id)
        {
            var user = await userStore.FindByIdAsync(Id, CancellationToken.None);
            if (user == null)
                return base.NotFound();
            await userStore.DeleteAsync(user, CancellationToken.None);
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {

            var userValid = await userValidator.IsValidUser(request.UserName, request.Password);
            if (!userValid)
                return UnprocessableEntity();

            var normalized = keyNormalizer.NormalizeName(request.UserName);
            var user = await userStore.FindByNameAsync(normalized, CancellationToken.None);
            if (user == null)
                return base.NotFound();

            var identity = await IdentityHelper.CreateNameIdentity(user);
            var jwt = jwtSignInHandler.BuildJwt(new ClaimsPrincipal(identity)).ToString();
            var json = JsonSerializer.Serialize(new { name = request.UserName, token = jwt });
            return Ok(json);
        }

        [HttpGet("Claims")]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            int i = 0;
            return Ok(new
            {
                Id = User.Identity.Name,
                Claims = User.Claims.ToDictionary(claim => (++i).ToString(), claim => claim.Value)
            });
        }
    }

    public static class IdentityHelper
    {
        public static async Task<ClaimsIdentity> CreateNameIdentity(UserRecord user)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            return identity;
        }

        public static async Task<ClaimsIdentity> CreateFullIdentity(UserRecord user, IRepository<GraphRecord> repository)
        {
            var identity = await CreateNameIdentity(user);
            foreach(var graph in await repository.AsQueryable())
            {
                //repository.Update(graph);
                //if (graph.Creator == user)
                //    user.Creations.Add(graph);
            }
            
            //updates graph repo
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
