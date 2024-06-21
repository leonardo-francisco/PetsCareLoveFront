namespace PetsCareLove.Web.ViewModels
{
    public class AppointmentViewModel
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public Guid PetId { get; set; }

        public Guid ServiceId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentStatus { get; set; }
        public string Notes { get; set; }


        public Guid? VeterinarianId { get; set; }
        public Guid? TrainerId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
