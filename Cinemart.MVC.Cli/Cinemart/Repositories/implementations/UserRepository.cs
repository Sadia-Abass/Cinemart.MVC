using Cinemart.Data;
using Cinemart.Models;
using Cinemart.Repositories.Interfaces;
using Cinemart.Services.Interfaces;
using Cinemart.ViewModels;
using Cinemart.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Cinemart.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private const int MaxPageSize = 100; 

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IFileUploaderService _fileUploaderService;
        private readonly ApplicationDbContext _applicationDbContext;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IFileUploaderService fileUploaderService, ApplicationDbContext applicationDbContext) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _fileUploaderService = fileUploaderService;
            _applicationDbContext = applicationDbContext;
        }


        // Creates a new user with password.
        // We rely on Identity's built-in uniqueness/validation (avoid extra pre-check round trip).
        public async Task<(IdentityResult Result, Guid? UserId)> CreateUserAsync(CreateUserViewModel createUserViewModel)
        {
            // ExecutionStrategy adds resiliency (automatic retries for transient SQL errors)
            var strategy = _applicationDbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync<(IdentityResult, Guid?)>(async () =>
            {
                // Start an explicit transaction
                await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

                try
                {
                    // Prepare a new ApplicationUser (keep UserName = Email for simplicity/consistency)
                    var user = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        Firstname = createUserViewModel.Firstname.Trim(),
                        Lastname = createUserViewModel.Lastname.Trim(),
                        Email = createUserViewModel.Email.Trim(),
                        UserName = createUserViewModel.Email.Trim(),
                        DOB = createUserViewModel.DOB,
                        IsActive = createUserViewModel.IsActive,
                        EmailConfirmed = createUserViewModel.MarkEmailConfirmed,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    // Let Identity enforce password policy + unique constraints (inside the transaction)
                    var create = await _userManager.CreateAsync(user, createUserViewModel.Password);

                    if (!create.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return (create, null);
                    }

                    await transaction.CommitAsync();
                    return (IdentityResult.Success, user.Id);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw; // let middleware/logging handle it; caller gets a 500
                }
            });
        }

        // Deletes a user with a guard to prevent removing the last Admin.
        public async Task<IdentityResult> DeleteUserAsync(Guid id)
        {
            var strategy = _applicationDbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync<IdentityResult>(async () =>
            {
                await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

                try
                {
                    var user = await _userManager.FindByIdAsync(id.ToString());
                    if (user == null)
                    {
                        await transaction.RollbackAsync();
                        return IdentityResult.Failed(new IdentityError
                        {
                            Code = "NotFound",
                            Description = "User not found."
                        });
                    }

                    // Safety: block deleting the last "Admin"
                    var adminRole = await _roleManager.FindByNameAsync("Admin");
                    if (adminRole != null)
                    {
                        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                        if (!isAdmin)
                        {
                            var anotherAdminExists = await _applicationDbContext.Set<IdentityUserRole<Guid>>()
                            .AnyAsync(ur => ur.RoleId == adminRole.Id && ur.UserId != user.Id);

                            if (!anotherAdminExists)
                            {
                                await transaction.RollbackAsync();
                                return IdentityResult.Failed(new IdentityError
                                {
                                    Code = "LastAdmin",
                                    Description = "You cannot delete the last user in the 'Admin' role."
                                });
                            }
                        }
                    }

                    var delete = await _userManager.DeleteAsync(user);
                    if (!delete.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return delete;
                    }

                    await transaction.CommitAsync();
                    return IdentityResult.Success;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }           
    


        // Returns a paged list of users with filter/search.
        // Uses normalized columns (index-friendly) where possible for best performance.
        public async Task<PagedResult<UserViewModel>> GetAllUsersAsync(UserListFilterViewModel filter)
        {
            // Normalize and clamp paging inputs to safe values
            var pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize < 1 ? 10 : (filter.PageSize > MaxPageSize ? MaxPageSize : filter.PageSize);

            // Base query (read-only fast path)
            var query = _userManager.Users.AsNoTracking();

            // Search heuristic:
            // - If it looks like an email, use NormalizedEmail (indexed).
            // - If it's numeric, filter by PhoneNumber prefix (common usage).
            // - Else, use NormalizedUserName (indexed) + First/Last name prefix.
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim();
                var searchUpper = search.ToUpperInvariant();

                if (search.Contains('@'))
                {
                    query = query.Where(u => u.NormalizedEmail!.StartsWith(searchUpper));
                }
                else
                {
                    query = query.Where(u => (u.NormalizedUserName!.StartsWith(searchUpper)) || (u.Firstname ?? "").StartsWith(search) || (u.Lastname ?? "").StartsWith(search));
                }               
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(u => u.EmailConfirmed == filter.EmailConfirmed.Value);
            }

            // Total count for pager (single scalar query)
            var total = await query.CountAsync();

            var items = await query  // Current sort: friendly alphabetical.
                .OrderBy(u => u.Firstname)
                .ThenBy(u => u.Lastname)
                .ThenBy(u => u.Email)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                // Project only what you need
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Email = u.Email!,
                    UserName = u.UserName!,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    IsActive = u.IsActive,
                    EmailConfirmed = u.EmailConfirmed,
                    CreatedAt = u.CreatedAt,
                }).ToListAsync();

            return new PagedResult<UserViewModel>
            {
                Items = items,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

      

        public async Task<EditUserViewModel?> GetForEditAsync(Guid id)
        {
            // AsNoTracking -> we don't need change tracking for display
            var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) 
            {
                return null;
            }

            return new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email!,
                DOB = user.DOB,
                imageUrl = user.imageUrl,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                ConcurrencyStamp = user.ConcurrencyStamp // used for optimistic concurrency in Update
            };
        }


        // Updates a user with optimistic concurrency check via ConcurrencyStamp.
        public async Task<IdentityResult> UpdateUserAsync(EditUserViewModel editUserViewModel)
        {
            // ExecutionStrategy adds resiliency (automatic retries for transient SQL errors)
            var straregy = _applicationDbContext.Database.CreateExecutionStrategy();
            
            return await straregy.ExecuteAsync<IdentityResult>(async () =>
            {
                await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

                try
                {
                    var user = await _userManager.FindByIdAsync(editUserViewModel.Id.ToString());
                    if(user == null)
                    {
                        await transaction.RollbackAsync();
                        return IdentityResult.Failed(new IdentityError
                        {
                            Code = "NotFound",
                            Description = "User not found."
                        });
                    }

                    // Optimistic concurrency guard:
                    // If stamp changed, someone else updated the record
                    if(!string.Equals(user.ConcurrencyStamp, editUserViewModel.ConcurrencyStamp, StringComparison.Ordinal))
                    {
                        await transaction.RollbackAsync();
                        return IdentityResult.Failed(new IdentityError
                        {
                            Code = "ConcurrencyFailure",
                            Description = "This user was modified by another admin. Please reload and try again."
                        });
                    }

                    // If email changed, update both Email & UserName (Identity will SaveChanges inside the transaction)
                    if(!string.Equals(user.Email, editUserViewModel.Email, StringComparison.Ordinal))
                    {
                        var emailResult = await _userManager.SetEmailAsync(user, editUserViewModel.Email.Trim());
                        if (!emailResult.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return emailResult;
                        }

                        var usernameResult = await _userManager.SetUserNameAsync(user, editUserViewModel.Email.Trim());
                        if (!usernameResult.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return usernameResult;
                        }
                    }

                    // Update profile fields
                    user.Firstname = editUserViewModel.FirstName.Trim();
                    user.Lastname = editUserViewModel.LastName?.Trim();
                    user.DOB = editUserViewModel.DOB;
                    user.IsActive = editUserViewModel.IsActive;
                    user.EmailConfirmed = editUserViewModel.EmailConfirmed;
                    user.UpdatedAt = DateTime.UtcNow;

                    var update = await _userManager.UpdateAsync(user);
                    if (update.Succeeded) 
                    {
                        await transaction.RollbackAsync();
                        return update;
                    }

                    await transaction.CommitAsync();
                    return IdentityResult.Success;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        // Returns detailed view model including assigned roles.
        public async Task<UserDetailsViewModel?> GetUserDetailsAsync(Guid id)
        {
            // Read-only entity for display
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            // Identity API requires the user entity for role lookup
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                DOB = user.DOB,
                imageUrl = user.imageUrl,
                LastLogin = user.LastLogin,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                CreatedDate = user.CreatedAt,
                ModifiedDate = user.UpdatedAt,
                Roles = roles.OrderBy(r => r).ToList(),
            };

        }


        // Builds the roles editor (checkbox list) with pre-checked assignments.// Builds the roles editor (checkbox list) with pre-checked assignments.
        public async Task<UserRolesEditViewModel?> GetRolesForEditAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) 
            {
                return null;
            }

            // List all active roles (read-only)
            var allRoles = await _roleManager.Roles
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .Where(r => r.IsActive)
                .ToListAsync();

            // Current assignments for the user
            var assignedRoles = await _userManager.GetRolesAsync(user);

            // Case-insensitive check to avoid surprises with different normalizations
            var userRolesEditViewModel = new UserRolesEditViewModel
            {
                UserId = user.Id,
                UserName = user.UserName!,
                Roles = allRoles.Select(role => new RoleCheckboxItem
                {
                    RoleId = role.Id,
                    RoleName = role.Name!,
                    Description = role.Description,
                    IsSelected = assignedRoles.Contains(role.Name!, StringComparer.OrdinalIgnoreCase)
                }).ToList()
            };

            return userRolesEditViewModel;
        }

        // Updates a user's roles using batched operations
        public async Task<IdentityResult> UpdateRolesAsync(Guid userId, IEnumerable<Guid> selectedRoleIds)
        {
            var strategy = _applicationDbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync<IdentityResult>(async () =>
            {
                await using var transanction = await _applicationDbContext.Database.BeginTransactionAsync();

                try
                {
                    var user = await _userManager.FindByIdAsync(userId.ToString());
                    if (user == null)
                    {
                        await transanction.RollbackAsync();
                        return IdentityResult.Failed(new IdentityError
                        {
                            Code = "NotFound",
                            Description = "User not found."
                        });
                    }

                    // Normalize and de-duplicate incoming IDs
                    var ids = (selectedRoleIds ?? Enumerable.Empty<Guid>()).Distinct().ToList();

                    // Map ONLY requested IDs -> names (read-only)
                    var selectedRoleNames = (ids.Count == 0) ? new List<string>() : await _roleManager.Roles.AsNoTracking().Where(r => ids.Contains(r.Id)).Select(r => r.Name!).ToListAsync();

                    // Validate existence
                    if (selectedRoleNames.Count != ids.Count)
                    {
                        await transanction.RollbackAsync();
                        return IdentityResult.Failed(new IdentityError
                        {
                            Code = "RoleNotFound",
                            Description = "One or more selected roles do not exist."
                        });
                    }

                    // Current roles
                    var currentRoles = await _userManager.GetRolesAsync(user);

                    // Compute diffs (case-insensitive)
                    var current = new HashSet<string>(currentRoles, StringComparer.OrdinalIgnoreCase);
                    var target = new HashSet<string>(selectedRoleNames, StringComparer.OrdinalIgnoreCase);

                    //current: Admin Manager User
                    //target: Admin Manager CustomerSupport Vendor
                    var toAdd = target.Except(current, StringComparer.OrdinalIgnoreCase).ToList(); // toAdd = CustomerSupport Vendor

                    var toRemove = current.Except(target, StringComparer.OrdinalIgnoreCase).ToList(); //toRemove = User


                    if (toAdd.Count() == 0 && toRemove.Count() == 0)
                    {
                        await transanction.CommitAsync(); // nothing to do
                        return IdentityResult.Success;
                    }

                    // Batch add/remove to minimize round-trips; both inside the same transaction
                    if (toAdd.Count() > 0)
                    {
                        var add = await _userManager.AddToRoleAsync(user, toAdd.ToString());
                        if (!add.Succeeded)
                        {
                            await transanction.RollbackAsync();
                            return add;
                        }
                    }

                    if (toRemove.Count() > 0)
                    {
                        var rem = await _userManager.RemoveFromRoleAsync(user, toRemove.ToString());
                        if (!rem.Succeeded)
                        {
                            await transanction.RollbackAsync();
                            return rem;
                        }
                    }

                    await transanction.CommitAsync();
                    return IdentityResult.Success;
                }
                catch
                {
                    await transanction.CommitAsync();
                    return IdentityResult.Success;
                }
            });
        }
    }
}
