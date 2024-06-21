namespace PetsCareLove.Web.ViewModels
{
    public class BreedViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TypeAnimalId { get; set; }
        public TypeAnimalViewModel TypeAnimal { get; set; }
    }
}
