using AirBnB.Data;
using AirBnB.Models;
using AirBnB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AirBnB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        //to control all users
        private readonly UserManager<AppUser> _UserManager;
        //to control all roles
        private readonly RoleManager<IdentityRole> _RoleManager;

        //injection ctor its parameter initialized by Dic that send them
        public UsersController(UserManager<AppUser> UserManager, RoleManager<IdentityRole> RoleManager)
        {
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }
        public IActionResult Index()
        {
            // but i don't need the all data of users i want the only data that in UserViewModel
            //code:
            //var users = await _UserManager.Users.ToListAsync();

            //then i selected wanted data only
            var users =_UserManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = _UserManager.GetRolesAsync(user).Result
            }).ToList();

             return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _RoleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleName = role.Name,
                    IsSelected = _UserManager.IsInRoleAsync(user, role.Name).Result,
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _UserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            //first condition if user check role (x) and he exactly has role (x) ==> no action
            //second condition if user check role (x) and he hasn't role (x) ==> add role
            //third condition if user unchecked role (x) and he exactly has't role (x) ==> no action
            //fourth condition if user unchecked role (x) and he has role (x) ==> remove role

            //first select the user with all his roles only
            var userRoles = await _UserManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                //fourth condition => user has this role but he unselected this role => remove role
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                {
                    await _UserManager.RemoveFromRoleAsync(user, role.RoleName);
                }
                //second condition => user hasn't this role but he selected this role => add role
                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                {
                    await _UserManager.AddToRoleAsync(user, role.RoleName);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        
    }
}
