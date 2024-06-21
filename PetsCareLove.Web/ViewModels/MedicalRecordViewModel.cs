namespace PetsCareLove.Web.ViewModels
{
    public class MedicalRecordViewModel
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public Guid VeterinarianId { get; set; }
        public Guid PetId { get; set; }
        public string MedicalHistory { get; set; }
        public string Medications { get; set; }
        public string Allergies { get; set; }
        public string Notes { get; set; }
    }
}
