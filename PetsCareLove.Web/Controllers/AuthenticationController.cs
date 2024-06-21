using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetsCareLove.Web.Dtos;
using PetsCareLove.Web.Services;
using PetsCareLove.Web.Validators;

namespace PetsCareLove.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly EmailService _emailService;

        public AuthenticationController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var validator = new UserDtoValidator();
            ValidationResult result = validator.Validate(loginDto);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(loginDto);
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var validator = new RegisterDtoValidator();
            ValidationResult result = validator.Validate(registerDto);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(registerDto);
            }

            await _emailService.SendEmailAsync("leonardosfrancisco@gmail.com","Bem Vindo");

            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(LoginDto loginDto)
        {
            var validator = new UserDtoValidator();
            ValidationResult result = validator.Validate(loginDto);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(loginDto);
            }

            return View();
        }
    }
}
