using System.Threading.Tasks;
using FirstApp.API.Models;

namespace FirstApp.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user , string pass);
         Task<User> LogIn(string userName , string pass);
         Task<bool> UserExist(string userName);

    }
}