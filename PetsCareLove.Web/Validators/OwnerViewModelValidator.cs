using FluentValidation;
using PetsCareLove.Web.ViewModels;

namespace PetsCareLove.Web.Validators
{
    public class OwnerViewModelValidator : AbstractValidator<OwnerViewModel>
    {
        public OwnerViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(2, 100).WithMessage("O nome deve ter entre 2 e 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail deve ser um endereço de e-mail válido.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("O telefone é obrigatório.");
                
        }
    }
}
