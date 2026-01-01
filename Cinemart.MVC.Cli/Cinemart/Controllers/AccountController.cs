using Cinemart.Data;
using Cinemart.Models;
using Cinemart.Services.Implementations;
using Cinemart.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cinemart.Controllers
{
    public class AccountController : Controller
    {
        private readonly FileUploaderService _fileUploaderService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(FileUploaderService fileUploaderService, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
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

        public async Task<IActionResult> Register(string returnUrl = null)
        {
            var registerViewModel = new RegisterViewModel();
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);
        }
    }
}
