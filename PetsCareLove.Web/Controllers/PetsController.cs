using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using PetsCareLove.Web.Dtos;
using PetsCareLove.Web.Services;
using PetsCareLove.Web.Validators;
using PetsCareLove.Web.ViewModels;

namespace PetsCareLove.Web.Controllers
{
    public class PetsController : Controller
    {
        private readonly PetService _petService;
        private readonly OwnerService _ownerService;

        public PetsController(PetService petService, OwnerService ownerService )
        {
            _petService = petService;
            _ownerService = ownerService;
        }

        public async Task<IActionResult> Index()
        {
            var pets = await _petService.GetAllPetsAsync();
            return View(pets);
        }

        public async Task<IActionResult> DetailsPet(Guid id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            var owner = pet != null ? await _ownerService.GetOwnerByIdAsync(pet.OwnerId) : null;

            InfoPetsViewModel viewModel = new InfoPetsViewModel
            {
                Pet = pet ?? new PetViewModel(),
                Owner = owner ?? new OwnerViewModel()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AddNewPet()
        {
            await FillDropDown();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPet(PetViewModel petViewModel)
        {
            await FillDropDown();

            var validator = new PetViewModelValidator();
            ValidationResult result = validator.Validate(petViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(petViewModel);
            }

            var response = await _petService.CreatePetAsync(petViewModel);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Animal criado com sucesso";
                return RedirectToAction("Index", "Pets");
            }

            ViewData["error"] = "Falha ao criar animal";
            return View(petViewModel);
        }
      
        public async Task<IActionResult> EditPet(Guid id)
        {
            await FillDropDown();
            var pet = await _petService.GetPetByIdAsync(id);
            return View(pet);
        }

        [HttpPost]
        public async Task<IActionResult> EditPet(Guid id, PetViewModel petViewModel)
        {
            await FillDropDown();

            var validator = new PetViewModelValidator();
            ValidationResult result = validator.Validate(petViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(petViewModel);
            }

            var response = await _petService.EditPetAsync(id, petViewModel);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Animal editado com sucesso";
                return RedirectToAction("Index", "Pets");
            }

            ViewData["error"] = "Falha ao editar animal";
            return View(petViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePet(Guid id)
        {
            try
            {
                await _petService.DeletePetAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private async Task FillDropDown()
        {
            ViewBag.Owner = new SelectList(await _ownerService.GetAllOwnersAsync(), "Id", "Name");
            ViewBag.Genero = new SelectList(await _petService.GetAllGendersAsync(), "Id", "Name");
            ViewBag.TipoAnimal = new SelectList(await _petService.GetAllTypeAnimalAsync(), "Id", "Name");
            ViewBag.Raca = new SelectList(await _petService.GetAllAnimalRaceAsync(), "Id", "Name");
        }
    }
}
