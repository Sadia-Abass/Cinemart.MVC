using Cinemart.Data;
using Cinemart.Models;
using Cinemart.Repositories.interfaces;
using Cinemart.ViewModels;
using Cinemart.ViewModels.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinemart.Repositories.implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public RoleRepository(RoleManager<ApplicationRole> roleManager, ApplicationDbContext applicationDbContext) 
        {
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
        }

        // Retrieves a paginated list of roles based on filter criteria.
        public async Task<PagedResult<RoleViewModel>> GetAllRolesAsync(RolefilterViewModel rolefilterViewModel)
        {
            // Start with all roles (No Tracking = better performance for read-only queries)
            var query = _roleManager.Roles.AsNoTracking();

            // Apply search filter(if provided)
            if (!string.IsNullOrWhiteSpace(rolefilterViewModel.Search))
            {
                var search = rolefilterViewModel.Search.Trim();
                query = query.Where(r => r.Name!.Contains(search) || (r.Description ?? "").Contains(search));
            }

            // Apply Active/Inactive filter (if provided)
            if (rolefilterViewModel.IsActive.HasValue)
            {
                query = query.Where(r => r.IsActive == rolefilterViewModel.IsActive.Value);
            }

            // Get total role count for pagination
            var total = await query.CountAsync();

            // Get current page of roles
            var items = await query
                .OrderBy(r => r.Name) // Sort alphabetically
                .Skip((rolefilterViewModel.PageNumber - 1) * rolefilterViewModel.PageSize) // Skip previous pages
                .Take(rolefilterViewModel.PageSize) // Take only required items
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name!,
                    Description = r.Description,
                    IsActive = r.IsActive,
                    CreatedDate = r.CreatedDate,
                }).ToListAsync();

            return new PagedResult<RoleViewModel>
            {
                Items = items,
                TotalCount = total,
                PageNumber = rolefilterViewModel.PageNumber,
                PageSize = rolefilterViewModel.PageSize
            };
        }

        // Creates a new role.
        public async Task<(IdentityResult Result, Guid? RoleId)> CreateRoleAsync(CreateRoleViewModel createRoleViewModel)
        {
            // Ensure the role name is unique
            bool roleExists = await _roleManager.RoleExistsAsync(createRoleViewModel.Name);
            if (roleExists) 
            {
                return (IdentityResult.Failed(new IdentityError
                {
                    Description = "Role name already exists."
                }), null);
            }

            // Create new ApplicationRole entity
            var role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = createRoleViewModel.Name.Trim(),
                NormalizedName = createRoleViewModel.Name.Trim().ToUpperInvariant(), // For case-insensitive comparison
                Description = createRoleViewModel.Description.Trim(),
                IsActive = createRoleViewModel.IsActive,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
            };

            // Save to database
            var result = await _roleManager.CreateAsync(role);
            return (result, result.Succeeded ? role.Id : null);
        }

        // Retrieves a role for editing.
        public async Task<EditRoleViewModel> GetRoleForEditAsync(Guid roleId)
        {
            var role = await _roleManager.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == roleId);
            if(role == null)
            {
                return null;
            }

            // Map to edit view model
            return new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                Description = role.Description,
                IsActive = role.IsActive,
                ConcurrencyStamp = role.ConcurrencyStamp // For concurrency checks
            };
        }

        // Updates an existing role.
        public async Task<IdentityResult> EditRoleAsync(EditRoleViewModel editRoleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(editRoleViewModel.Id.ToString());
            if (role == null) 
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "NotFound",
                    Description = "Role not found."
                });  
            }

            // Concurrency check — prevents overwriting changes made by others
            if (!string.Equals(role.ConcurrencyStamp, editRoleViewModel.ConcurrencyStamp, StringComparison.Ordinal))
            {
                return IdentityResult.Failed(new IdentityError 
                {
                    Code = "ConcurrencyFailure",
                    Description = "This role was modified by another user while you were editing. Please reload the page and try again."
                });
            }

            // Ensure name is still unique (excluding current role)
            if (!string.Equals(role.Name, editRoleViewModel.Name, StringComparison.Ordinal))
            {
                var duplicate = await _roleManager.FindByNameAsync(editRoleViewModel.Name);
                if (duplicate != null && duplicate.Id != role.Id)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "DuplicateRoleName",
                        Description = $"Another role already uses this name: {editRoleViewModel.Name}"
                    });
                }
            }

            // Update properties
            role.Name = editRoleViewModel.Name.Trim();
            role.NormalizedName = editRoleViewModel.Name.ToUpperInvariant();
            role.Description = editRoleViewModel.Description;
            role.IsActive = editRoleViewModel.IsActive;
            role.ModifiedDate = DateTime.UtcNow;

            // Save changes — updates ConcurrencyStamp automatically
            return await _roleManager.UpdateAsync(role);
        }


        // Deletes a role if it has no assigned users.
        public async Task<IdentityResult> DeleteRoleAsync(Guid roleId)
        {
           var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) 
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Role not found"
                });
            }

            // Prevent deletion if any users are assigned to this role
            var hasUsers = await _applicationDbContext.Set<IdentityRole<Guid>>()
                .AsNoTracking()
                .AnyAsync(ur => ur.Id == roleId);

            if (hasUsers) 
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Cannot delete a role that still has users. Remove users from the role first."
                });
            }

            // Delete role
            return await _roleManager.DeleteAsync(role);
        }


        // Retrieves details of a role, including paginated list of users in that role.
        public async Task<RoleDetailsViewModel?> GetRoleDetailsAsync(Guid roleId, int pageNumber, int pageSize)
        {
            var role = await _roleManager.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null) 
            {
                return null;
            }

            // Query all users in this role via junction table (IdentityUserRole)
            var userQuery = from ur in _applicationDbContext.Set<IdentityUserRole<Guid>>().AsNoTracking() //Left table - User Roles
                            join u in _applicationDbContext.Set<ApplicationUser>().AsNoTracking() //Right table - Users
                            on ur.UserId equals u.Id
                            select new UserInRoleViewModel
                            {
                                Id = u.Id,
                                Email = u.Email,
                                Firstname = u.Firstname,
                                Lastname = u.Lastname,
                                IsActive = u.IsActive
                            };

            // Get total user count
            var total = await userQuery.CountAsync();

            // Get current page of users
            var users = await userQuery
                .OrderBy(u => u.Email)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return role details with users
            return new RoleDetailsViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsActive = role.IsActive,
                CreatedDate = role.CreatedDate,
                ModifiedDate = role.ModifiedDate,
                Users = new PagedResult<UserInRoleViewModel>
                {
                    Items = users,
                    TotalCount = total,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            };
        }
    }
}
