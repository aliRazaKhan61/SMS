using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SMS.Data;
using SMS.Infrastructure.DTOs;
using System.Data;

namespace SMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserRoleController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModels>();
            foreach (IdentityUser user in users)
            {
                var thisViewModel = new UserRolesViewModels();
                thisViewModel.UserId = users.First().Id;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
        public async Task<IActionResult> Manage(string Id)
        {
            string userId = Id;
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            var userRoles = await _userManager.GetRolesAsync(user);
            
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (userRoles.Contains(role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Edit(string UserId)
        {
            IdentityUser user = await _userManager.FindByIdAsync(UserId);
            EditUserDto editUser=new EditUserDto();
            editUser.Email = user.Email;
            editUser.Id = user.Id;
            editUser.PhoneNumber = user.PhoneNumber;
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleId = await _roleManager.FindByNameAsync(userRoles.First());
            editUser.RoleId = roleId.Id;
            var roles = await _roleManager.Roles.ToListAsync();
            ViewData["RoleId"] = new SelectList(roles, "Id", "Name", editUser.RoleId);
            return View(editUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserDto input)
        {
            if (id != input.Id)
            {
                return NotFound();
            }
            IdentityResult result = null;
            if (ModelState.IsValid)
            {
                try
                {
                    IdentityUser user =await _userManager.FindByNameAsync(input.Email);
                    user.NormalizedUserName=input.Email.ToUpper();
                    user.Email = input.Email;
                    user.Id = input.Id;
                    user.PhoneNumber = input.PhoneNumber;
                    user.UserName = input.Email;
                    user.NormalizedEmail=input.Email.ToUpper();
                    string password= _userManager.PasswordHasher.HashPassword(user, input.Password);
                    user.PasswordHash=password;

                    //var user = new IdentityUser { UserName = input.Email, NormalizedUserName = input.Email.ToUpper(), Email = input.Email, NormalizedEmail = input.Email.ToUpper(), PhoneNumber = input.PhoneNumber ,PasswordHash=input.Password};
                    string token=await getToken(user);
                    //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        
                    //if (!String.IsNullOrWhiteSpace(input.Password) && input.Password == input.ConfirmPassword)
                    //{
                    //    await _userManager.RemovePasswordAsync(user);
                    //    await _userManager.AddPasswordAsync(user, input.Password);
                    //    //result = await _userManager.ResetPasswordAsync(user, token, input.Password);
                    //}
                    result = await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool exist = await AspNetUsersExists(input.Id);
                    if (!exist)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(input);
        }
        public async Task<string> getToken(IdentityUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        private async Task< bool> AspNetUsersExists(string id)
        {
            IdentityUser user=await _userManager.FindByIdAsync(id);
            if(user == null)
                return false;
            else
                return true;
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return View(user);
        }

        //POST: AspNetUser/Delete/5
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        public async Task<JsonResult> DeleteConfirmed(string id)
        {
            var user =await _userManager.FindByIdAsync(id);
                var result=await _userManager.DeleteAsync(user);
            if(result.Succeeded) 
            {
                await _signInManager.SignOutAsync();
                return Json(result);
            }
            return Json(result);
        }
    }
}


