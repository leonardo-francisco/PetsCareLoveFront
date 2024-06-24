using PetsCareLove.Web.Dtos;

namespace PetsCareLove.Web.Response
{
    public class LoginResponse
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
