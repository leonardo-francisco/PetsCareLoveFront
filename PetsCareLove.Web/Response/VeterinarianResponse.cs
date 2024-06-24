using PetsCareLove.Web.ViewModels;

namespace PetsCareLove.Web.Response
{
    public class VeterinarianResponse
    {
        public string Message { get; set; }
        public VeterinarianViewModel Veterinarian { get; set; }
    }
}
