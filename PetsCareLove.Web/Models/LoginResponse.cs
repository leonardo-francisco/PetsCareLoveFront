using PetsCareLove.Web.Dtos;

namespace PetsCareLove.Web.Models
{
    public class LoginResponse
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
