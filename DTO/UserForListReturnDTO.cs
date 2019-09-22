using System;
using System.Collections.Generic;
using FirstApp.API.Models;

namespace FirstApp.API.DTO
{
    public class UserForListReturnDTO
    {
    
        public int Id {get;set;}
        public string Username {get;set;}
        public string Gender {get;set;}
        public int  Age {get;set;}

        public string KnownAs {get;set;}
        public DateTime Created {get;set;}
        public DateTime LastActive {get;set;}

        public string Introduction {get;set;}
        public string City {get;set;}
        public string Country {get;set;}
        public string MainImageURL {get;set;}
        public ICollection<PhotoForListReturnDTO> Photos {get;set;}
        

    }
}