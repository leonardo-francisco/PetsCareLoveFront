namespace PetsCareLove.Web.ViewModels
{
	public class RoleViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public List<Guid>? PermissionsIds { get; set; } = new List<Guid>();
	}
}
