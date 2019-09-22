using System.Collections.Generic;
using System.Threading.Tasks;
using FirstApp.API.Models;

namespace FirstApp.API.Data
{
    public interface IUserRepository
    {
         void add<T>(T entity) where T:class;
         void delete<T>(T entity) where T:class;
         Task<User> getUser(int Id);
         Task<IEnumerable<User>> getUsers();
         Task<bool> saveAll();

    }
}