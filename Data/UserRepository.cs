using System.Collections.Generic;
using System.Threading.Tasks;
using FirstApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstApp.API.Data
{
    public class UserRepository : IUserRepository
    {
        DataContext context;
        public UserRepository(DataContext ctx)
        {
            context=ctx;
        }
        public void add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public async Task<User> getUser(int Id)
        {
            return await context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id==Id);
        }

        public async Task<IEnumerable<User>> getUsers()
        {
            return await context.Users.Include(u => u.Photos).ToListAsync();
        }

        public async Task<bool> saveAll()
        {
            return await context.SaveChangesAsync() > 0 ;
        }
    }
}