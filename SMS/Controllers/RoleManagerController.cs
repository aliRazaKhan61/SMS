using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName,string roleId)
        {
            if (roleId == null)
            {
                //add
                if (roleName != null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
                }
            }
            else
            {
                //update
                IdentityRole role =await _roleManager.FindByIdAsync(roleId);
                role.Name= roleName;
                role.NormalizedName= roleName.ToUpper();
                await _roleManager.UpdateAsync(role);
            }
           
            return RedirectToAction("Index");
        }
    }
}
