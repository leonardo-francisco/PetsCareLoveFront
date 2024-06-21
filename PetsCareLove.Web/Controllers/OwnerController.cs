using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetsCareLove.Web.Services;
using PetsCareLove.Web.Validators;
using PetsCareLove.Web.ViewModels;

namespace PetsCareLove.Web.Controllers
{
    public class OwnerController : Controller
    {
        private readonly OwnerService _ownerService;
        private readonly PetService _petService;

        public OwnerController(OwnerService ownerService, PetService petService)
        {
            _ownerService = ownerService;
            _petService = petService;
        }
        public async Task<IActionResult> Index()
        {
            var owners = await _ownerService.GetAllOwnersAsync();
            return View(owners);
        }

        public async Task<IActionResult> DetailsOwner(Guid id)
        {
            var owner = await _ownerService.GetOwnerByIdAsync(id);
            var pets = new List<PetViewModel>();

            if (owner != null && owner.PetIds != null)
            {
                foreach (var petId in owner.PetIds)
                {
                    var pet = await _petService.GetPetByIdAsync(petId);
                    if (pet != null)
                    {
                        pets.Add(new PetViewModel
                        {
                            Id = pet.Id,
                            Name = pet.Name,
                            BreedName = pet.BreedName,
                            TypeAnimalName = pet.TypeAnimalName,
                            Photo = pet.Photo
                        });
                    }
                }
            }

            InfoOwnersViewModel viewModel = new InfoOwnersViewModel
            {
                Owner = owner ?? new OwnerViewModel(),
                Pets = pets
            };

            return View(viewModel);
        }

        public IActionResult AddNewOwner()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewOwner(OwnerViewModel ownerViewModel)
        {
            var validator = new OwnerViewModelValidator();
            ValidationResult result = validator.Validate(ownerViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(ownerViewModel);
            }

            var response = await _ownerService.CreateOwnerAsync(ownerViewModel);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Tutor criado com sucesso";
                return RedirectToAction("Index", "Owner");
            }

            ViewData["error"] = "Falha ao criar tutor";
            return View(ownerViewModel);
        }

        public async Task<IActionResult> EditOwner(Guid id)
        {
            var owner = await _ownerService.GetOwnerByIdAsync(id);
            return View(owner);
        }

        [HttpPost]
        public async Task<IActionResult> EditOwner(Guid id,OwnerViewModel ownerViewModel)
        {
            var validator = new OwnerViewModelValidator();
            ValidationResult result = validator.Validate(ownerViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(ownerViewModel);
            }

            var response = await _ownerService.EditOwnerAsync(id,ownerViewModel);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Tutor editado com sucesso";
                return RedirectToAction("Index", "Owner");
            }

            ViewData["error"] = "Falha ao editar tutor";
            return View(ownerViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            try
            {
                await _ownerService.DeleteOwnerAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
