using FluentValidation;
using Microsoft.Extensions.Localization;
using PetsCareLove.Web.Dtos;
using PetsCareLove.Web.ViewModels;

namespace PetsCareLove.Web.Validators
{
    public class PetViewModelValidator : AbstractValidator<PetViewModel>
    {
        public PetViewModelValidator()
        {
            RuleFor(pet => pet.Name).NotEmpty().WithMessage("O nome do animal é obrigatório.");
                                    
            RuleFor(pet => pet.DateOfBirth).NotEmpty().WithMessage("A data de nascimento é obrigatória.");

            RuleFor(pet => pet.TypeAnimalId)
                .NotEmpty().WithMessage("O tipo de animal é obrigatório.")
                .NotEqual(Guid.Empty).WithMessage("O tipo de animal não pode ser vazio.")
                .Must(id => id != Guid.Empty).WithMessage("O tipo de animal é obrigatório.");

            RuleFor(pet => pet.BreedId).NotEmpty().WithMessage("A raça é obrigatória.")
                                       .NotEqual(Guid.Empty).WithMessage("A raça não pode ser vazia.");

            RuleFor(pet => pet.GenderId).NotEmpty().WithMessage("O gênero é obrigatório.")
                                        .NotEqual(Guid.Empty).WithMessage("O gênero não pode ser vazio.");

            RuleFor(pet => pet.OwnerId).NotEmpty().WithMessage("O tutor é obrigatória.")
                                       .NotEqual(Guid.Empty).WithMessage("O tutor não pode ser vazia.");
        }
    }
}
