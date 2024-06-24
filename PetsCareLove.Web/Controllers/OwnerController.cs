using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetsCareLove.Web.Response;
using PetsCareLove.Web.Services;
using PetsCareLove.Web.Validators;
using PetsCareLove.Web.ViewModels;
using System.Text.Json;

namespace PetsCareLove.Web.Controllers
{
    public class OwnerController : Controller
    {
        private readonly OwnerService _ownerService;
        private readonly PetService _petService;
        private readonly PhotoService _photoService;
        public OwnerController(OwnerService ownerService, PetService petService, PhotoService photoService)
        {
            _ownerService = ownerService;
            _petService = petService;
            _photoService = photoService;

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
                var ownerResponse = await DeserializeResponseAsync<OwnerResponse>(response);
                if (ownerResponse == null)
                {
                    TempData["error"] = "Falha ao criar tutor";
                    return View(ownerViewModel);
                }

                var imagePathFromApi = GetApiImagePath(ownerResponse.Owner.Id);
                var localImagePath = GetLocalImagePath(ownerResponse.Owner.Id);

                if (await DownloadAndSaveImageAsync(imagePathFromApi, localImagePath))
                {
                    TempData["success"] = "Tutor criado com sucesso";
                    return RedirectToAction("Index", "Owner");
                }
                else
                {
                    TempData["error"] = "Falha ao salvar a imagem do tutor";
                    return View(ownerViewModel);
                }
            }

            TempData["error"] = "Falha ao criar tutor";
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

            TempData["error"] = "Falha ao editar tutor";
            return View(ownerViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            var owner = await _ownerService.GetOwnerByIdAsync(id);

            if (owner != null)
            {
                if (!string.IsNullOrEmpty(owner.Photo))
                {
                    string photoPath = Path.Combine("wwwroot", owner.Photo.TrimStart('/'));
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }

                await _ownerService.DeleteOwnerAsync(owner.Id);
            }
            return Ok();
        }

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
        private string GetApiImagePath(Guid ownerId)
        {
            return $"https://localhost:7166/images/owner/{ownerId}.jpg";
        }

        // Helper method to get the local image path
        private string GetLocalImagePath(Guid ownerId)
        {
            string directoryPath = Path.Combine("wwwroot", "images", "owner");
            Directory.CreateDirectory(directoryPath);
            return Path.Combine(directoryPath, $"{ownerId}.jpg");
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
