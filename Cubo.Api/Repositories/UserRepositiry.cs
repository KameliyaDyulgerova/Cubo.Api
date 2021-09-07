using System;
using System.Collections.Generic;
using System.Linq;
using Cubo.Api.Models;

namespace Cubo.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISet<User> _users = new HashSet<User>
        {
            new User(1, "user1", "user1@myapp.com"),
            new User(2, "user2", "user2@myapp.com"),
            new User(3, "user3", "user3@myapp.com"),
        };

        public User Get(string name)
        => _users.First(x => x.Name == name);

        public IEnumerable<User> GetAll()
        => _users;
    }
}
