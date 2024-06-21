using FluentValidation;
using PetsCareLove.Web.Dtos;

namespace PetsCareLove.Web.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            Include(new UserDtoValidator());

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(100).WithMessage("Nome não pode exceder 100 caracteres.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Perfil é obrigatório.");
        }
    }
}
