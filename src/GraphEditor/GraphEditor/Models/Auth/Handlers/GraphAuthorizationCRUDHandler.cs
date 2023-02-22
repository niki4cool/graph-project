using GraphEditor.Models.Auth.User;
using GraphEditor.Models.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace GraphEditor.Models.Auth.Handlers
{
    public class GraphAuthorizationCRUDHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, GraphRecord>
    {
        private readonly IUserStore<UserRecord> userStore;

        public GraphAuthorizationCRUDHandler(IUserStore<UserRecord> userStore)
        {
            this.userStore = userStore;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   OperationAuthorizationRequirement requirement,
                                                   GraphRecord resource)
        {
            Console.WriteLine(">>> - " + requirement.Name);

            if (context.User.Claims.Any(c => c.Value == resource.ViewRole)
                && requirement.Name == Operations.Read.Name)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
