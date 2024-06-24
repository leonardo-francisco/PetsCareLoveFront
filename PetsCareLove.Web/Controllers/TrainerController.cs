using FluentValidation;
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
    public class TrainerController : Controller
    {
        private readonly TrainerService _trainerService;
        private readonly PhotoService _photoService;
        private readonly OwnerService _ownerService;
        private readonly PetService _petService;
        private readonly EmployeeService _employeeService;

        public TrainerController(TrainerService trainerService, PhotoService photoService, OwnerService ownerService, PetService petService, EmployeeService employeeService)
        {
            _trainerService = trainerService;
            _photoService = photoService;
            _ownerService = ownerService;
            _petService = petService;
            _employeeService = employeeService;
        }
        public async Task<IActionResult> Index()
        {
            var tngs = await _trainerService.GetAllTrainersAsync();
            return View(tngs);
        }

        public async Task<IActionResult> DetailsTrainer(Guid id)
        {
            var trainer = await _trainerService.GetTrainerByIdAsync(id);
            var appointmentsInfo = new List<AppointmentInfoViewModel>();

            foreach (var appointment in trainer.Appointments)
            {
                var owner = await _ownerService.GetOwnerByIdAsync(appointment.OwnerId);
                var pet = await _petService.GetPetByIdAsync(appointment.PetId);
                var service = await _employeeService.GetServiceByIdAsync(appointment.ServiceId);

                appointmentsInfo.Add(new AppointmentInfoViewModel
                {
                    Id = appointment.Id,
                    Owner = new OwnerViewModel
                    {
                        Id = owner.Id,
                        Name = owner.Name
                    },
                    Pet = new PetViewModel
                    {
                        Id = pet.Id,
                        Name = pet.Name,
                        BreedName = pet.BreedName,
                        Photo = pet.Photo
                    },
                    Service = new ServiceViewModel
                    {
                        Id = service.Id,
                        Description = service.Description
                    },
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentStatus = appointment.AppointmentStatus,
                    Notes = appointment.Notes,
                    TrainerId = appointment.TrainerId,
                    EmployeeId = appointment.EmployeeId
                });
            }

            var viewModel = new InfoTrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,               
                Email = trainer.Email,
                Phone = trainer.Phone,
                Photo = trainer.Photo,
                AppointmentsInfo = appointmentsInfo
            };

            return View(viewModel);
        }

        public IActionResult AddNewTrainer(Guid id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTrainer(TrainerViewModel trainerViewModel)
        {
            var validator = new TrainerViewModelValidator();
            ValidationResult result = validator.Validate(trainerViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(trainerViewModel);
            }

            var response = await _trainerService.CreateTrainerAsync(trainerViewModel);

            if (response.IsSuccessStatusCode)
            {
                var tngResponse = await DeserializeResponseAsync<TrainerResponse>(response);
                if (tngResponse == null)
                {
                    TempData["error"] = "Falha ao criar adestrador";
                    return View(trainerViewModel);
                }

                var imagePathFromApi = GetApiImagePath(tngResponse.Trainer.Id);
                var localImagePath = GetLocalImagePath(tngResponse.Trainer.Id);

                if (await DownloadAndSaveImageAsync(imagePathFromApi, localImagePath))
                {
                    TempData["success"] = "Adestrador criado com sucesso";
                    return RedirectToAction("Index", "Trainer");
                }
                else
                {
                    TempData["error"] = "Falha ao salvar a imagem do adestrador";
                    return View(trainerViewModel);
                }
            }

            TempData["error"] = "Falha ao criar adestrador";
            return View(trainerViewModel);
        }

        public async Task<IActionResult> EditTrainer(Guid id)
        {
            var tng = await _trainerService.GetTrainerByIdAsync(id);
            return View(tng);
        }

        [HttpPost]
        public async Task<IActionResult> EditTrainer(Guid id, TrainerViewModel trainerViewModel)
        {
            var validator = new TrainerViewModelValidator();
            ValidationResult result = validator.Validate(trainerViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(trainerViewModel);
            }

            var response = await _trainerService.EditTrainerAsync(id, trainerViewModel);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Adestrador atualizado com sucesso";
                return RedirectToAction("Index", "Trainer");
            }

            TempData["error"] = "Falha ao editar adestrador";
            return View(trainerViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTrainer(Guid id)
        {
            var tng = await _trainerService.GetTrainerByIdAsync(id);

            if (tng != null)
            {
                if (!string.IsNullOrEmpty(tng.Photo))
                {
                    string photoPath = Path.Combine("wwwroot", tng.Photo.TrimStart('/'));
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }

                await _trainerService.DeleteTrainerAsync(tng.Id);
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
        private string GetApiImagePath(Guid trainerId)
        {
            return $"https://localhost:7226/images/trainer/{trainerId}.jpg";
        }

        // Helper method to get the local image path
        private string GetLocalImagePath(Guid trainerId)
        {
            string directoryPath = Path.Combine("wwwroot", "images", "trainer");
            Directory.CreateDirectory(directoryPath);
            return Path.Combine(directoryPath, $"{trainerId}.jpg");
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
