using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.DTO
{
    public class UserAuthRegisterDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        [StringLength(12, MinimumLength=4, ErrorMessage="Password must be between 4 and 8")]
        public string password {get; set;}
    }
}