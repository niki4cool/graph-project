using GraphEditor.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace GraphEditor.Auth
{
    public class UserRecordValidator
    {
        private readonly IUserStore<User> userStore;
        private readonly ILookupNormalizer keyNormalizer;
        private readonly byte[] salt;

        public UserRecordValidator(IUserStore<User> userStore, ILookupNormalizer keyNormalizer)
        {
            this.userStore = userStore;
            this.keyNormalizer = keyNormalizer;

            salt = Encoding.UTF8.GetBytes("saltysalt");
        }

        public async Task<IdentityResult> CreateAsync(string userName, string password)
        {
            var user = new User()
            {
                Name = userName,                
                NormalizedName = keyNormalizer.NormalizeName(userName),
                PasswordHash = HashPassword(password),
            };
            await userStore.CreateAsync(user, CancellationToken.None);
            return IdentityResult.Success;
        }

        public async Task<bool> IsValidUser(string userName, string password)
        {
            var hash = HashPassword(password);
            var normalized = keyNormalizer.NormalizeName(userName);
            var user = await userStore.FindByNameAsync(normalized, CancellationToken.None);
            if (user == null)
                return false;
            if (hash != user.PasswordHash)
                return false;
            return true;
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 100, 256 / 8));
        }
    }
}
