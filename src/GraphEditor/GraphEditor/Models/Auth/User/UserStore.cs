using GraphEditor.Models.CRUD;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System.Buffers.Text;
using System.Security.AccessControl;

namespace GraphEditor.Models.Auth.User
{
    public class UserStore : IUserStore<UserRecord>, IUserPasswordStore<UserRecord>
    {
        private readonly IRepository<UserRecord> repository;
        private readonly ILookupNormalizer keyNormalizer;

        public UserStore(IRepository<UserRecord> repository, ILookupNormalizer keyNormalizer)
        {
            this.repository = repository;
            this.keyNormalizer = keyNormalizer;
        }

        public async Task<IdentityResult> CreateAsync(UserRecord user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await repository.Add(user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(UserRecord user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await repository.Delete(user.Id);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public async Task<UserRecord?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await repository.Find(userId);
        }

        public async Task<UserRecord?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var users = await repository.AsQueryable();
            return users.Where(u => u.NormalizedName == normalizedUserName)
                                        .FirstOrDefault();
        }

        public async Task<string?> GetNormalizedUserNameAsync(UserRecord user, CancellationToken cancellationToken)
        {
            var entity = await FindByIdAsync(user.Id, cancellationToken);
            return entity?.NormalizedName;
        }

        public async Task<string?> GetPasswordHashAsync(UserRecord user, CancellationToken cancellationToken)
        {
            var entity = await FindByIdAsync(user.Id, cancellationToken);
            return entity?.PasswordHash;
        }

        public Task<string> GetUserIdAsync(UserRecord user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public async Task<string?> GetUserNameAsync(UserRecord user, CancellationToken cancellationToken)
        {
            var entity = await FindByIdAsync(user.Id, cancellationToken);
            return entity?.UserName;
        }

        public async Task<bool> HasPasswordAsync(UserRecord user, CancellationToken cancellationToken)
        {
            var entity = await FindByIdAsync(user.Id, cancellationToken);
            return string.IsNullOrEmpty(entity?.PasswordHash);
        }

        public async Task SetNormalizedUserNameAsync(UserRecord user, string? normalizedName, CancellationToken cancellationToken)
        {
            if (normalizedName == null)
                throw new ArgumentNullException(nameof(normalizedName));
            user.NormalizedName = normalizedName;
            await repository.Update(user);
        }

        public async Task SetPasswordHashAsync(UserRecord user, string? passwordHash, CancellationToken cancellationToken)
        {
            if (passwordHash == null)
                throw new ArgumentNullException(nameof(passwordHash));
            user.PasswordHash = passwordHash;
            await repository.Update(user);
        }

        public async Task SetUserNameAsync(UserRecord user, string? userName, CancellationToken cancellationToken)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));
            user.UserName = userName;
            await SetNormalizedUserNameAsync(user, keyNormalizer.NormalizeName(userName), cancellationToken);
            await repository.Update(user);
        }

        public async Task<IdentityResult> UpdateAsync(UserRecord user, CancellationToken cancellationToken)
        {
            await repository.Update(user);
            return IdentityResult.Success;
        }
    }
}
