
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FirstApp.API.Data;
using FirstApp.API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FirstApp.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]   
    public class UserController:ControllerBase
    {
        private IUserRepository userRepo;
        private IMapper mapper;
        public UserController(IUserRepository repo,IMapper mapper)
        {
            userRepo=repo;
            this.mapper=mapper;
        }

        [HttpGet]
        public async Task<IActionResult> getUsers(){

           var users = await userRepo.getUsers();
           var usersToReturn = mapper.Map<IEnumerable<UserForListReturnDTO>>(users);
           return Ok(usersToReturn);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> getUser(int Id){

            var user = await userRepo.getUser(Id);
            var userToReturn = mapper.Map<UserForListReturnDTO>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> updateUser(int id,UserForUpdateDto user){

            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             return Unauthorized();

            var userFromRepo = await userRepo.getUser(id);
            mapper.Map(user,userFromRepo);

            if(await userRepo.saveAll())
               return NoContent();

            throw new Exception($"user with id: {id} failed to update");

            
        }
    }   

    
}