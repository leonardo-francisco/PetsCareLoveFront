namespace PetsCareLove.Web.ViewModels
{
    public class OwnerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Photo { get; set; }
        public List<Guid>? PetIds { get; set; } = new List<Guid>();
    }
}
