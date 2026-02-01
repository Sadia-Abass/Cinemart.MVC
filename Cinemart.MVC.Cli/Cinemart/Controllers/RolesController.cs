using Cinemart.Repositories.interfaces;
using Cinemart.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemart.Controllers
{
    //[Authorize]
    public class RolesController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleRepository roleRepository, ILogger<RolesController> logger)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        // GET: /Roles
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]RolefilterViewModel rolefilterViewModel)
        {
            try
            {
                var result = await _roleRepository.GetAllRolesAsync(rolefilterViewModel);
                ViewBag.Filter = rolefilterViewModel;
                return View(result);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error fetching roles in Index action.");
                return View("Error");
            }
           
        }

        // GET: /Roles/Create
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                return View(new CreateRoleViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rendering Create Role form.");
                return View("Error");
            }
        }

        // POST: /Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel createRoleViewModel)
        {
            try
            {
                // DataAnnotations validation first
                if (!ModelState.IsValid)
                {
                    return View(createRoleViewModel);
                }

               var (result, id) = await _roleRepository.CreateRoleAsync(createRoleViewModel);
                if (result.Succeeded)
                {
                    TempData["Success"] = $"Role '{createRoleViewModel.Name}' created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                // Map IdentityResult errors to MODEL-LEVEL errors
                foreach (var error in result.Errors) 
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(createRoleViewModel);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error creating role '{RoleName}'.", createRoleViewModel?.Name);
                return View("Error");
            }
        }

        // GET: /Roles/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var viewModel = await _roleRepository.GetRoleForEditAsync(id);
                if(viewModel == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching role for edit. RoleId: {RoleId}", id);
                return View("Error");
            }
        }

        // POST: /Roles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleViewModel editRoleViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(editRoleViewModel);
                }

                var result = await _roleRepository.EditRoleAsync(editRoleViewModel);
                if (result.Succeeded)
                {
                    TempData["Success"] = $"Role '{editRoleViewModel.Name}' updated successfully.";
                    return RedirectToAction(nameof(Index));
                }

                // Map IdentityResult errors to MODEL-LEVEL errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(editRoleViewModel);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error updating role '{RoleName}'.", editRoleViewModel?.Name);
                return View("Error");
            }
        }


        // GET: /Roles/Details/{id}?page=1&pageSize=4
        [HttpGet]
        public async Task<IActionResult> Details(Guid id, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var viewModel = await _roleRepository.GetRoleDetailsAsync(id, pageNumber, pageSize);
                if (viewModel == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching role details. RoleId: {RoleId}", id);
                return View("Error");
            }
        }

        // POST: /Roles/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _roleRepository.DeleteRoleAsync(id);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Role deleted";
                }
                else
                {
                    TempData["Error"] = string.Join(" ", result.Errors.Select(e => e.Description));
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role. RoleId: {RoleId}", id);
                return View("Error");
            }
        }
    }
}
