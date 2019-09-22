using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FirstApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {

        private readonly DataContext context;
        public AuthRepository(DataContext context)
        {
         this.context = context;   
        }

        public async Task<User> LogIn(string userName, string pass)
        {
            User user = await context.Users.FirstOrDefaultAsync(userx => userx.Username==userName);
            if(user==null){
                return  null;
            }
            if(verifyPassHash(user,pass)){
                return user;
            }
            return null;
        
        }

        private bool verifyPassHash(User user, string pass)
        {
            byte[] passHash;
            using(var hmac = new HMACSHA512(user.PasswordSalt)){

                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
                for(int i=0;i<passHash.Length;i++){
                    if(passHash[i] != user.PasswordHash[i]){
                        return false;
                    }
                }

            }
            return true;
        }

        public async Task<User> Register(User user, string pass)
        {
            byte[] passHash,passSalt;
            createPassHashAndSalt(pass,out passHash,out passSalt);
            user.PasswordHash = passHash ;
            user.PasswordSalt = passSalt ;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        private void createPassHashAndSalt(string pass, out byte[] passHash, out byte[] passSalt)
        {
            using(var hmac = new HMACSHA512()){
                
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));

            }
        }

        public async Task<bool> UserExist(string userName)
        {
            if(await context.Users.AnyAsync(user => user.Username==userName)){
                return true;
            }else{
                return false;
            }
            
        }
    }
}