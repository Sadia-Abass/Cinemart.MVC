using Cinemart.Data;
using Cinemart.Models;
using Cinemart.Services.Implementations;
using Cinemart.Services.Interfaces;
using Cinemart.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cinemart.Controllers
{
    public class AccountController : Controller
    {
        private readonly IFileUploaderService _fileUploaderService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(IFileUploaderService fileUploaderService, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _fileUploaderService = fileUploaderService;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            var registerViewModel = new RegisterViewModel();
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
        {
            registerViewModel.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");

            var userImage = await _fileUploaderService.AddFileAsync(registerViewModel.ImageUrl);

            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = new ApplicationUser
            {
                Firstname = registerViewModel.Firstname,
                Lastname = registerViewModel.Lastname,
                UserName = registerViewModel.Email,
                NormalizedUserName = registerViewModel.Email.ToUpper(),
                Email = registerViewModel.Email,
                NormalizedEmail = registerViewModel.Email.ToUpper(),
                DOB = registerViewModel.DOB,
                imageUrl = userImage.SecureUrl.ToString(),
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync("Member");

                if (!roleExists)
                {
                    var role = new ApplicationRole 
                    { 
                        Name = "Member",
                        Description = "Customer"
                    };

                    await _roleManager.CreateAsync(role);
                }

                await _userManager.AddToRoleAsync(user, "Member");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach(var error in result.Errors)
            {
                //ModelState.AddModelError("Password", "User could not be created. Password not unique enough.");
                ModelState.AddModelError(string.Empty, error.Description);
            }
           
            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            var loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded) 
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                return RedirectToAction("Lockout");
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
            return View(loginViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
    }
}
