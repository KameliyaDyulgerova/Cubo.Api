using System.Collections.Generic;
using Cubo.Api.Models;

namespace Cubo.Api.Repositories
{
    public interface IUserRepository
    {
        User Get(string name);
        IEnumerable<User> GetAll();
    }
}