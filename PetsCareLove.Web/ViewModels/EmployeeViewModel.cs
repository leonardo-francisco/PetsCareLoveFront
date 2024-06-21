namespace PetsCareLove.Web.ViewModels
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string? RegistrationCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ServiceType { get; set; }
        public List<Guid>? AppointmentIds { get; set; } = new List<Guid>();
    }
}
