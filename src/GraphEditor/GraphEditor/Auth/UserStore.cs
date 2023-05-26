using GraphEditor.CRUD;
using GraphEditor.Model;
using GraphEditor.Model.GraphModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GraphEditor.Auth
{
    public class UserStore : IUserStore<User>, IUserPasswordStore<User>
    {
        private readonly GraphDBContext context;
        private readonly ILookupNormalizer keyNormalizer;


        public UserStore(GraphDBContext context, ILookupNormalizer keyNormalizer)
        {
            this.context = context;
            this.keyNormalizer = keyNormalizer;
        }

        private IIncludableQueryable<User, List<Graph>> QueryWithInclusions()
        {
            return context.Users.Include(e => e.CanView)
                                .Include(e => e.CanEdit)
                                .Include(e => e.Creations);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public void Dispose() {}

        public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken) 
            => await QueryWithInclusions().Where(e => e.Id == userId)
                                          .FirstOrDefaultAsync(cancellationToken);

        public async Task<User?> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await QueryWithInclusions().Where(e => e.NormalizedName == keyNormalizer.NormalizeName(name))
                                          .FirstOrDefaultAsync(cancellationToken);
        }
            

        public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.NormalizedName);

        public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);


        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id);


        public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.Name);

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            var entity = await FindByIdAsync(user.Id, cancellationToken);
            return string.IsNullOrEmpty(entity?.PasswordHash);
        }

        public async Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedName = normalizedName!;
            context.Update(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash!;
            context.Update(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        {
            user.Name = userName!;
            await SetNormalizedUserNameAsync(user, keyNormalizer.NormalizeName(userName), cancellationToken);
            context.Update(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            context.Update(user);
            await context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }
    }
}
