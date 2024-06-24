using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using PetsCareLove.Web.Dtos;
using PetsCareLove.Web.Response;
using PetsCareLove.Web.Services;
using PetsCareLove.Web.Validators;
using PetsCareLove.Web.ViewModels;
using System.Text.Json;

namespace PetsCareLove.Web.Controllers
{
    public class PetsController : Controller
    {
        private readonly PetService _petService;
        private readonly OwnerService _ownerService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PhotoService _photoService;

        public PetsController(PetService petService, OwnerService ownerService, IWebHostEnvironment webHostEnvironment, PhotoService photoService )
        {
            _petService = petService;
            _ownerService = ownerService;
            _webHostEnvironment = webHostEnvironment;
            _photoService = photoService;
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
                var petResponse = await DeserializeResponseAsync<PetResponse>(response);
                if (petResponse == null)
                {
                    TempData["error"] = "Falha ao criar animal";
                    return View(petViewModel);
                }

                var imagePathFromApi = GetApiImagePath(petResponse.Pet.Id);
                var localImagePath = GetLocalImagePath(petResponse.Pet.Id);

                if (await DownloadAndSaveImageAsync(imagePathFromApi, localImagePath))
                {
                    TempData["success"] = "Animal criado com sucesso";
                    return RedirectToAction("Index", "Pets");
                }
                else
                {
                    TempData["error"] = "Falha ao salvar a imagem do animal";
                    return View(petViewModel);
                }
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

            TempData["error"] = "Falha ao editar animal";
            return View(petViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePet(Guid id)
        {
            var pet = await _petService.GetPetByIdAsync(id);

            if (pet != null)
            {
                if (!string.IsNullOrEmpty(pet.Photo))
                {
                    string photoPath = Path.Combine("wwwroot", pet.Photo.TrimStart('/'));
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }

                await _petService.DeletePetAsync(pet.Id);
            }
           
            return Ok();
        }    

        #region DropDown
        private async Task FillDropDown()
        {
            ViewBag.Owner = new SelectList(await _ownerService.GetAllOwnersAsync(), "Id", "Name");
            ViewBag.Genero = new SelectList(await _petService.GetAllGendersAsync(), "Id", "Name");
            ViewBag.TipoAnimal = new SelectList(await _petService.GetAllTypeAnimalAsync(), "Id", "Name");
            ViewBag.Raca = new SelectList(await _petService.GetAllAnimalRaceAsync(), "Id", "Name");
        }
        #endregion

        #region Imagem
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(new { success = false, error = "No file uploaded." });

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            //var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var imagePath = $"https://localhost:7031/uploads/{uniqueFileName}";
                return Json(new { success = true, filePath = imagePath });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // Helper method to deserialize response
        private async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Helper method to get the image path from the API
        private string GetApiImagePath(Guid petId)
        {
            return $"https://localhost:7196/images/pets/{petId}.jpg";
        }

        // Helper method to get the local image path
        private string GetLocalImagePath(Guid petId)
        {
            string directoryPath = Path.Combine("wwwroot", "images", "pets");
            Directory.CreateDirectory(directoryPath);
            return Path.Combine(directoryPath, $"{petId}.jpg");
        }

        // Helper method to download and save the image
        private async Task<bool> DownloadAndSaveImageAsync(string sourceUrl, string destinationPath)
        {
            try
            {
                await _photoService.DownloadAndSaveImageAsync(sourceUrl, destinationPath);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error downloading or saving image.");
                return false;
            }
        }
        #endregion

    }
}
