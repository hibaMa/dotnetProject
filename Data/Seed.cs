using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using FirstApp.API.Models;
using Newtonsoft.Json;

namespace FirstApp.API.Data
{
    //add users to the data base from users json file - called in programe.cs
    public class Seed
    {
        public static void SeedUsers(DataContext context){
            if(!context.Users.Any()){
                var userData = System.IO.File.ReadAllText("Data/UserData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach(var user in users){
                    byte[] passwordHash , passwordSalt;
                    createPassHashAndSalt("password" , out passwordHash , out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();
                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
            
        }


        private static void createPassHashAndSalt(string pass, out byte[] passHash, out byte[] passSalt)
        {
            using(var hmac = new HMACSHA512()){
                
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));

            }
        }
        
    }
}