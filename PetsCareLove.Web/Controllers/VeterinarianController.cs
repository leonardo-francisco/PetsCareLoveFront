using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetsCareLove.Web.Services;
using PetsCareLove.Web.Validators;
using PetsCareLove.Web.ViewModels;

namespace PetsCareLove.Web.Controllers
{
    public class VeterinarianController : Controller
    {
        private readonly VeterinarianService _veterinarianService;
        private readonly OwnerService _ownerService;
        private readonly PetService _petService;
        private readonly EmployeeService _employeeService;

        public VeterinarianController(VeterinarianService veterinarianService, OwnerService ownerService, PetService petService, EmployeeService employeeService)
        {
            _veterinarianService = veterinarianService;
            _ownerService = ownerService;
            _petService = petService;
            _employeeService = employeeService;

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
                TempData["success"] = "Veterinário criado com sucesso";
                return RedirectToAction("Index", "Veterinarian");
            }

            ViewData["error"] = "Falha ao criar veterinário";
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

            ViewData["error"] = "Falha ao editar veterinário";
            return View(veterinarianViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVeterinarian(Guid id)
        {
            try
            {
                await _veterinarianService.DeleteVeterinarianAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
    }
}
