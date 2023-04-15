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
        private IServiceScope serviceScope;
        private IUserStore<UserRecord> userStore;
        private IRepository<GraphRecord> graphRepository;
        private ILookupNormalizer keyNormalizer;
        public GraphAuthorizationCRUDHandler(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            serviceScope = serviceScopeFactory.CreateScope();
            userStore = serviceScope.ServiceProvider.GetRequiredService<IUserStore<UserRecord>>();
            graphRepository = serviceScope.ServiceProvider.GetRequiredService<IRepository<GraphRecord>>();
            keyNormalizer = serviceScope.ServiceProvider.GetRequiredService<ILookupNormalizer>();
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                           OperationAuthorizationRequirement requirement,
                                           GraphRecord resource)
        {            
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
