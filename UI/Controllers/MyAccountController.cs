using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using e_commerce.Domain.Entites;
using e_commerce.Infrastructure.Entites;
using e_commerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace e_commerce.Web.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignInManager;

        public MyAccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View(); 
        }

        public async Task<IActionResult> UpdateProfile()
        {
            var user = await UserManager.GetUserAsync(User);
            var model = new UpdateProfileViewModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await UserManager.GetUserAsync(User);
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                ViewBag.ProfileMessage = "Profile updated successfully!";
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await UserManager.GetUserAsync(User);
            var result = await UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await SignInManager.RefreshSignInAsync(user);
                ViewBag.PasswordMessage = "Password changed successfully!";
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }

}
