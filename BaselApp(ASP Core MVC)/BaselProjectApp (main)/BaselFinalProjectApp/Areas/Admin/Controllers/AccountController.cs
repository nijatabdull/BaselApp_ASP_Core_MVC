using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Models.MainModel;
using BaselFinalProjectApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaselFinalProjectApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class AccountController : Controller
    {
        private BaselDbContext _baselDb { get; set; }
        private SignInManager<AppUser> _signInManager { get; set; }
        private UserManager<AppUser> _userManager { get; set; }

        public AccountController(BaselDbContext baselDb,
                                  SignInManager<AppUser> signInManager,
                                  UserManager<AppUser> userManager)
        {
            _baselDb = baselDb;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IList<AppUser> appUsers = await _userManager.GetUsersInRoleAsync("User");

            HttpContext.Session.SetInt32("UserCount", appUsers.Count);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                IList<AppUser> appUsers = await _userManager.GetUsersInRoleAsync("Admin");

                AppUser appUser = appUsers.FirstOrDefault();

                if (appUser != null)
                {
                    appUser.UserName = registerModel.Username;
                    appUser.Email = registerModel.Email;

                    string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

                    await _userManager.ResetPasswordAsync(appUser,token,registerModel.Password);

                    await _userManager.UpdateAsync(appUser);

                    ViewBag.Message = "Update Successfully";
                }
                else
                    ModelState.AddModelError("", "User is not found");
            }

            return View(nameof(Index));
        }

        [HttpGet("Account/Index/{active}/{Id}")]
        public async Task<IActionResult> ChangeActive(bool active,string Id)
        {
            if (Id!=null)
            {
                AppUser appUser = await _userManager.FindByIdAsync(Id);

                if (appUser!=null)
                {
                    if (active)
                        appUser.LockoutEnd = DateTime.UtcNow.AddYears(50);
                    else
                        appUser.LockoutEnd = null;

                    await _userManager.UpdateAsync(appUser);
                }
                else
                    ModelState.AddModelError("", "User is not found");
            }
            else
                ModelState.AddModelError("", "Incorrect Information");

            return View(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account",new { area=""});
        }
    }
}