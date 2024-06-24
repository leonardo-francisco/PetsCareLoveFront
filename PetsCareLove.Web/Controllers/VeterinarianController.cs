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
    public class VeterinarianController : Controller
    {
        private readonly VeterinarianService _veterinarianService;
        private readonly OwnerService _ownerService;
        private readonly PetService _petService;
        private readonly EmployeeService _employeeService;
        private readonly PhotoService _photoService;

        public VeterinarianController(VeterinarianService veterinarianService, OwnerService ownerService, PetService petService, EmployeeService employeeService, PhotoService photoService)
        {
            _veterinarianService = veterinarianService;
            _ownerService = ownerService;
            _petService = petService;
            _employeeService = employeeService;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var vets = await _veterinarianService.GetAllVeterinariansAsync();
            return View(vets);
        }

        public async Task<IActionResult> DetailsVeterinarian(Guid id)
        {
            var veterinarian = await _veterinarianService.GetVeterinarianByIdAsync(id);
            var appointmentsInfo = new List<AppointmentInfoViewModel>();

            foreach (var appointment in veterinarian.Appointments)
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

            var viewModel = new InfoVeterinarianViewModel
            {
                Id = veterinarian.Id,
                Name = veterinarian.Name,
                Crmv = veterinarian.Crmv,
                Email = veterinarian.Email,
                Phone = veterinarian.Phone,
                Photo = veterinarian.Photo,
                AppointmentsInfo = appointmentsInfo
            };

            return View(viewModel);
        }

        public IActionResult AddNewVeterinarian(Guid id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewVeterinarian(VeterinarianViewModel veterinarianViewModel)
        {         
            var validator = new VeterinarianViewModelValidator();
            ValidationResult result = validator.Validate(veterinarianViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(veterinarianViewModel);
            }

            var response = await _veterinarianService.CreateVeterinarianAsync(veterinarianViewModel);

            if (response.IsSuccessStatusCode)
            {
                var vetResponse = await DeserializeResponseAsync<VeterinarianResponse>(response);
                if (vetResponse == null)
                {
                    TempData["error"] = "Falha ao criar veterinário";
                    return View(veterinarianViewModel);
                }

                var imagePathFromApi = GetApiImagePath(vetResponse.Veterinarian.Id);
                var localImagePath = GetLocalImagePath(vetResponse.Veterinarian.Id);

                if (await DownloadAndSaveImageAsync(imagePathFromApi, localImagePath))
                {
                    TempData["success"] = "Veterinário criado com sucesso";
                    return RedirectToAction("Index", "Veterinarian");
                }
                else
                {
                    TempData["error"] = "Falha ao salvar a imagem do veterinário";
                    return View(veterinarianViewModel);
                }
            }

            TempData["error"] = "Falha ao criar veterinário";
            return View(veterinarianViewModel);
        }

        public async Task<IActionResult> EditVeterinarian(Guid id)
        {
            var vet = await _veterinarianService.GetVeterinarianByIdAsync(id);
            return View(vet);
        }

        [HttpPost]
        public async Task<IActionResult> EditVeterinarian(Guid id,VeterinarianViewModel veterinarianViewModel)
        {
            var validator = new VeterinarianViewModelValidator();
            ValidationResult result = validator.Validate(veterinarianViewModel);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(veterinarianViewModel);
            }

            var response = await _veterinarianService.EditVeterinarianAsync(id,veterinarianViewModel);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Veterinário atualizado com sucesso";
                return RedirectToAction("Index", "Veterinarian");
            }

            TempData["error"] = "Falha ao editar veterinário";
            return View(veterinarianViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVeterinarian(Guid id)
        {
            var vet = await _veterinarianService.GetVeterinarianByIdAsync(id);

            if (vet != null)
            {
                if (!string.IsNullOrEmpty(vet.Photo))
                {
                    string photoPath = Path.Combine("wwwroot", vet.Photo.TrimStart('/'));
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }

                await _veterinarianService.DeleteVeterinarianAsync(vet.Id);
            }

            return Ok();
        }

        public async Task<IActionResult> MedicalRecordsByAppointment(Guid id)
        {
            var medicalRecord = await _veterinarianService.GetMedicalRecordByAppointmentIdAsync(id);
            var veterinarian = await _veterinarianService.GetVeterinarianByIdAsync(medicalRecord.VeterinarianId);
            var pet = await _petService.GetPetByIdAsync(medicalRecord.PetId);
            var owner = await _ownerService.GetOwnerByIdAsync(pet.OwnerId);

            InfoMedicalRecordViewModel viewModel = new InfoMedicalRecordViewModel();
            viewModel.MedicalRecord = medicalRecord;
            viewModel.Veterinarian = veterinarian;
            viewModel.Pet = pet;
            viewModel.Owner = owner;

            return View(viewModel);
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
        private string GetApiImagePath(Guid veterinarianId)
        {
            return $"https://localhost:7088/images/veterinarian/{veterinarianId}.jpg";
        }

        // Helper method to get the local image path
        private string GetLocalImagePath(Guid veterinarianId)
        {
            string directoryPath = Path.Combine("wwwroot", "images", "veterinarian");
            Directory.CreateDirectory(directoryPath);
            return Path.Combine(directoryPath, $"{veterinarianId}.jpg");
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
