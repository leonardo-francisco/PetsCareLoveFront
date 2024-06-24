using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetsCareLove.Web.Dtos;
using PetsCareLove.Web.Response;
using PetsCareLove.Web.Services;
using PetsCareLove.Web.Validators;
using System.Text.Json;

namespace PetsCareLove.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly EmailService _emailService;
        private readonly AuthService _authService;   

        public AuthenticationController(EmailService emailService, AuthService authService)
        {
            _emailService = emailService;
			_authService = authService;      
		}

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var validator = new LoginDtoValidator();
            ValidationResult result = validator.Validate(loginDto);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(loginDto);
            }

            var response = await _authService.LoginAsync(loginDto);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                HttpContext.Session.SetString("User", JsonSerializer.Serialize(loginResponse.User));
                HttpContext.Session.SetString("Token", JsonSerializer.Serialize(loginResponse.Token));
                return RedirectToAction("Index", "Dashboard");
            }

            var errorResponseBody = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorResponseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            TempData["error"] = errorResponse.Message.ToString();
            return View(loginDto);
        }

        public async Task<IActionResult> Register()
        {
            await FillDropDown();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            await FillDropDown();

            var validator = new UserDtoValidator();
            ValidationResult result = validator.Validate(userDto);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(userDto);
            }
			var response = await _authService.RegisterAsync(userDto);
            if (response.IsSuccessStatusCode)
            {
				await _emailService.SendEmailAsync(userDto.Email, "Bem Vindo");
                TempData["success"] = "Usuário registrado com sucesso";
                return View(userDto);  
			}

            var errorResponseBody = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorResponseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            TempData["error"] = errorResponse.Message.ToString();
            return View(userDto);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(LoginDto loginDto)
        {
            var validator = new LoginDtoValidator();
            ValidationResult result = validator.Validate(loginDto);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState, null);
                return View(loginDto);
            }

            var response = await _authService.RecoveryPasswordAsync(loginDto);
            if (response.IsSuccessStatusCode)
            {               
                TempData["success"] = "Senha alterada com sucesso";
                return View(loginDto);
            }

            var errorResponseBody = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorResponseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            TempData["error"] = errorResponse.Message.ToString();
            return View(loginDto);
        }

        [HttpPost]
        public IActionResult Logout()
        {         
            HttpContext.Session.Clear();          
            return RedirectToAction("Login", "Authentication");
        }

        private async Task FillDropDown()
        {
            ViewBag.Roles = new SelectList(await _authService.GetAllRolesAsync(), "Id", "Name");            
        }
    }
}
