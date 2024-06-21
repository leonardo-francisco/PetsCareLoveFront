using FluentValidation;
using PetsCareLove.Web.Dtos;

namespace PetsCareLove.Web.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("Formato de e-mail inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");
        }
    }
}
