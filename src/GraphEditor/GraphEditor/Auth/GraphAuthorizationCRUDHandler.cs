using GraphEditor.Model.GraphModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace GraphEditor.Auth
{
    public class GraphAuthorizationCRUDHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Graph>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                           OperationAuthorizationRequirement requirement,
                                           Graph resource)
        {
            var claims = context.User.Claims.ToList();

            var canView = claims.Any(c => c.Value == resource.ViewRole);
            var canEdit = claims.Any(c => c.Value == resource.EditRole);
            canView |= canEdit;

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

            return Task.CompletedTask;
        }
    }
}
