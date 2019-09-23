using System.Linq;
using AutoMapper;
using FirstApp.API.DTO;
using FirstApp.API.Models;

namespace FirstApp.API.helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User,UserForListReturnDTO>()
            .ForMember(dest => dest.MainImageURL,opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
            .ForMember(dest => dest.Age,opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())); 
 
            CreateMap<Photo,PhotoForListReturnDTO>(); 
            CreateMap<UserForUpdateDto,User>(); 
            CreateMap<PhotoForCreationDto,Photo>();
        }
    }
}