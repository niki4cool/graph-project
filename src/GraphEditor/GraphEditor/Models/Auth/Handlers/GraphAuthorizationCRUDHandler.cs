using GraphEditor.Controllers.User;
using GraphEditor.Models.Auth.User;
using GraphEditor.Models.CRUD;
using GraphEditor.Models.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GraphEditor.Models.Auth.Handlers
{
    public class GraphAuthorizationCRUDHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, GraphRecord>
    {
        private IServiceScopeFactory serviceScopeFactory;


        public GraphAuthorizationCRUDHandler(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                           OperationAuthorizationRequirement requirement,
                                           GraphRecord resource)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<UserRecord>>();
                var graphRepository = scope.ServiceProvider.GetRequiredService<IRepository<GraphRecord>>();
                var keyNormalizer = scope.ServiceProvider.GetRequiredService<ILookupNormalizer>();
                var normalized = keyNormalizer.NormalizeName(context.User.Identity?.Name);
                if (normalized == null)
                {
                    context.Fail();
                    return;
                }
                var user = await userStore.FindByNameAsync(normalized,
                                                CancellationToken.None);
                if (user == null)
                {
                    context.Fail();
                    return;
                }
                var claims = (await IdentityHelper.CreateFullIdentity(user, graphRepository)).Claims;

                var canView = claims.Any(c => c.Value == resource.ViewRole);
                var canEdit = claims.Any(c => c.Value == resource.EditRole);
                canView |= canEdit; //Editors can view

                if (requirement.Name == Operations.Create.Name)
                    context.Succeed(requirement);
                else if (requirement.Name == Operations.Read.Name && canView)
                    context.Succeed(requirement);
                else if (requirement.Name == Operations.Update.Name && canEdit)
                    context.Succeed(requirement);
                else if (requirement.Name == Operations.Delete.Name && canEdit)
                    context.Succeed(requirement);
                else
                    context.Fail();

                return;
            }
        }
    }
}
