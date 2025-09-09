using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Controllers
{
    [Authorize(Roles = "Admin")] // protect the whole controller - not just individual functions - because we don't want standard users accessing user data
    public class UserManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager; // we use the built-in functionality for user management
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: UserManagement
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    // we need less information on the index page 
                    Id = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return View(userViewModels);
        }

        // GET: UserManagement/Create
        public async Task<IActionResult> Create()
        {
            var allRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();
            var viewModel = new UserCreateViewModel
            {
                AvailableRoles = allRoles
            };
            return View(viewModel);
        }

        // POST: UserManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = model.EmailConfirmed
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Assign roles if any are selected
                    if (model.SelectedRoles.Any())
                    {
                        await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                    }

                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Reload available roles if model is invalid
            model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();
            return View(model);
        }

        // GET: UserManagement/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles.ToList()
            };

            return View(viewModel);
        }

        // GET: UserManagement/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();

            var viewModel = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
                SelectedRoles = roles.ToList(),
                AvailableRoles = allRoles
            };

            return View(viewModel);
        }

        // POST: UserManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Update basic user properties
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.EmailConfirmed = model.EmailConfirmed;

                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    // Update roles
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var rolesToRemove = currentRoles.Except(model.SelectedRoles).ToList();
                    var rolesToAdd = model.SelectedRoles.Except(currentRoles).ToList();

                    if (rolesToRemove.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    }

                    if (rolesToAdd.Any())
                    {
                        await _userManager.AddToRolesAsync(user, rolesToAdd);
                    }

                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Reload available roles if model is invalid
            model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();
            return View(model);
        }

        // GET: UserManagement/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles.ToList()
            };

            return View(viewModel);
        }

        // POST: UserManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Delete", new UserViewModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        EmailConfirmed = user.EmailConfirmed,
                        Roles = (await _userManager.GetRolesAsync(user)).ToList()
                    });
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }

    // we define custom view models for security and flexibility
    // binding data direcrtly to the user entity class provided by ASP.NET core is bad security practice - this only exposes what we want (nothing dangeous)
    // we can also expand the models to provide role management - not built-in
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class UserEditViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> SelectedRoles { get; set; } = new();
        public List<string> AvailableRoles { get; set; } = new();
    }

    public class UserCreateViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public List<string> SelectedRoles { get; set; } = new();
        public List<string> AvailableRoles { get; set; } = new();
    }
}
