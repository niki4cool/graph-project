using GraphEditor.Models.Auth.User;
using GraphEditor.Models.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace GraphEditor.Models.Auth.Handlers
{
    public class GraphAuthorizationCRUDHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, GraphRecord>
    {
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                           OperationAuthorizationRequirement requirement,
                                           GraphRecord resource)
        {
            var userName = context.User.Identity?.Name;
            if (userName == null)
            {
                context.Fail();
                return;
            }
            var user = await userStore.FindByNameAsync(userName,
                                            CancellationToken.None);

            var canView = context.User.Claims.Any(c =>
                                            c.Value == resource.ViewRole);
            var canEdit = context.User.Claims.Any(c =>
                        c.Value == resource.EditRole);
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
