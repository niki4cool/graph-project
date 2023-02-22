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

namespace GraphEditor.Controllers.User
{

    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserStore<UserRecord> userStore;
        private readonly UserRecordValidator userValidator;
        private readonly ILookupNormalizer keyNormalizer;

        public UserController(IUserStore<UserRecord> userStore,
                              ILookupNormalizer keyNormalizer,
                              UserRecordValidator userValidator )
        {
            this.userStore = userStore;
            this.keyNormalizer = keyNormalizer;
            this.userValidator = userValidator;
        }

        [HttpPut]
        public async Task<ActionResult> Create([FromBody] CreateUserRequest request)
        {
            var result = await userValidator.CreateAsync(request.UserName, request.Email, request.Password);
            if (result == null || !result.Succeeded)
                return base.BadRequest();
            return await GetByName(request.UserName);            
        }

        [HttpGet("{userName}")]
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

        [HttpGet("{Id}")]
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

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            foreach (var graph in user.CanEdit)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, graph.EditRole));
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok();
        }

    }
}
