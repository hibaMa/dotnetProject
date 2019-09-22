using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using FirstApp.API.Data;
using FirstApp.API.DTO;
using FirstApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FirstApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthenticationController:ControllerBase
    {
        private readonly IAuthRepository authRep;
        private readonly IConfiguration Configuration;
        public AuthenticationController(IAuthRepository rep,IConfiguration Conf)
        {
            this.authRep=rep;
            this.Configuration = Conf;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userdto){
            
            userdto.userName=userdto.userName.ToLower();
            User user = await authRep.LogIn(userdto.userName,userdto.password);
            if(user==null)
            return Unauthorized();

            //add user id and name to the token
            var claims = new []{
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Username) 
            };
            
            //get key used to sign the  token
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("AppSitting:Token").Value));
            //hash the key
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                 Subject = new ClaimsIdentity(claims),
                 Expires = DateTime.Now.AddDays(1),
                 SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new{
                Token = tokenHandler.WriteToken(token)
            });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userdto){

            userdto.userName=userdto.userName.ToLower();
            if(await authRep.UserExist(userdto.userName)){
               return BadRequest("user name already exist"); 
            }
              
            User userToCreat = new User{
                Username = userdto.userName
            };

            User createdUser = await authRep.Register(userToCreat,userdto.password);
            return StatusCode(201);
        }
    }
}