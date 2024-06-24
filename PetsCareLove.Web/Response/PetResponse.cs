using PetsCareLove.Web.ViewModels;

namespace PetsCareLove.Web.Response
{
    public class PetResponse
    {
        public string Message { get; set; }
        public PetViewModel Pet { get; set; }
    }
}
