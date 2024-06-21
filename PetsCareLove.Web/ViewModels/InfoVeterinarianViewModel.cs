namespace PetsCareLove.Web.ViewModels
{
    public class InfoVeterinarianViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Crmv { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public List<AppointmentInfoViewModel> AppointmentsInfo { get; set; }
    }

    public class AppointmentInfoViewModel
    {
        public Guid Id { get; set; }
        public OwnerViewModel Owner { get; set; }
        public PetViewModel Pet { get; set; }
        public ServiceViewModel Service { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentStatus { get; set; }
        public string Notes { get; set; }
        public Guid? TrainerId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
