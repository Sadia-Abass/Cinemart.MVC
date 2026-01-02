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

        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            var registerViewModel = new RegisterViewModel();
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
        {
            registerViewModel.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Firstname = registerViewModel.Firstname,
                    Lastname = registerViewModel.Lastname,
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    DOB = registerViewModel.DOB,
                    imageUrl = registerViewModel.ReturnUrl
                };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded) 
                { 
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                ModelState.AddModelError("Password", "User could not be created. Password not unique enough.");
            }
            return View(registerViewModel);
        }
    }
}
