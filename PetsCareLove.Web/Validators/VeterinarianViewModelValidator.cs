using FluentValidation;
using PetsCareLove.Web.ViewModels;

namespace PetsCareLove.Web.Validators
{
    public class VeterinarianViewModelValidator : AbstractValidator<VeterinarianViewModel>
    {
        public VeterinarianViewModelValidator()
        {
            RuleFor(v => v.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

            RuleFor(v => v.Crmv)
                .NotEmpty().WithMessage("O CRMV é obrigatório.")
                .Matches(@"^CRMV\/[A-Z]{2} \d{4,6}$").WithMessage("O CRMV deve estar no formato CRMV/UF NNNN .");

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email deve ser válido.");

            RuleFor(v => v.Phone)
                .NotEmpty().WithMessage("O telefone é obrigatório.")
                .Matches(@"^\(?\d{2}\)?[\s-]?\d{4,5}-?\d{4}$").WithMessage("O telefone deve ser válido (exemplo: (XX) XXXXX-XXXX).");
        }
    }
}
