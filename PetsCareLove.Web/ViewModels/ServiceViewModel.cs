using Newtonsoft.Json;

namespace PetsCareLove.Web.ViewModels
{
    public class ServiceViewModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string DurationString { get; set; }
    }
}
