using System.ComponentModel.DataAnnotations;

namespace FirstApp.API.DTO
{
    public class UserForRegisterDTO
    {
        [Required]
        public string userName{get;set;}
        [StringLength(8,MinimumLength=4,ErrorMessage="password must be between 4-8 letters")]
        public string password{get;set;}
    }
}