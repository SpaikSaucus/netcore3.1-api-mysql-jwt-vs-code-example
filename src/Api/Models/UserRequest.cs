using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class UserRequest
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string email { get; set; }
    }
}
