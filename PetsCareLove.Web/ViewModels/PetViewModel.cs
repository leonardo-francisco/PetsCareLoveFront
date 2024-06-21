namespace PetsCareLove.Web.ViewModels
{
    public class PetViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Photo { get; set; }
        public Guid TypeAnimalId { get; set; }
        public string? TypeAnimalName { get; set; }
        public Guid BreedId { get; set; }
        public string? BreedName { get; set; }
        public Guid GenderId { get; set; }
        public string? GenderName { get; set; }
        public Guid OwnerId { get; set; }
        public string? OwnerName { get; set; }
    }
}
