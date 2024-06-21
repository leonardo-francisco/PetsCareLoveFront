namespace PetsCareLove.Web.ViewModels
{
    public class InfoMedicalRecordViewModel
    {
        public MedicalRecordViewModel MedicalRecord { get; set; }
        public VeterinarianViewModel Veterinarian { get; set; }
        public PetViewModel Pet { get; set; }   
        public OwnerViewModel Owner { get; set; }
    }
}
