using PetsCareLove.Web.ViewModels;

namespace PetsCareLove.Web.Response
{
    public class TrainerResponse
    {
        public string Message { get; set; }
        public TrainerViewModel Trainer { get; set; }
    }
}
