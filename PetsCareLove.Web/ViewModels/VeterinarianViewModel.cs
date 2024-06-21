namespace PetsCareLove.Web.ViewModels
{
    public class VeterinarianViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Crmv { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Photo { get; set; }
        public List<AppointmentViewModel>? Appointments { get; set; }
    }
}
