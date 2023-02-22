using GraphEditor.Models.CRUD;
using Microsoft.EntityFrameworkCore;

namespace GraphEditor.Models.Auth.User
{
    public class UserRepository : RecordRepositoryBase<UserRecord>
    {
        public UserRepository(GraphDBContext context) : base(context)
        {
        }
    }
}
