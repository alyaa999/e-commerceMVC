using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using e_commerce.Domain.Entites;
using e_commerce.Infrastructure.Entites;
using e_commerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using e_commerce.Application.Common.Interfaces;


namespace e_commerce.Web.Controllers
{
    [ServiceFilter(typeof(LayoutDataFilterAttribute))]
    [Authorize]

    public class MyAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly IEmailSenderService _emailSender;

        public MyAccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSenderService emailSender)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _emailSender = emailSender;
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





        [HttpGet]
        public IActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await UserManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var token = await UserManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);

            var confirmLink = Url.Action("ConfirmNewEmail", "MyAccount", new
            {
                userId = user.Id,
                newEmail = model.NewEmail,
                token = token
            }, protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(model.NewEmail, "Confirm your new email",
                          $"Click <a href='{confirmLink}'>here</a> to confirm your new email address.");

            ViewBag.EmailMessage = "A confirmation link has been sent to your new email.";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmNewEmail(string userId, string newEmail, string token)
        {
            if (userId == null || newEmail == null || token == null)
                return View("Error");

            var user = await UserManager.FindByIdAsync(userId);
            if (user == null) return View("Error");

            var result = await UserManager.ChangeEmailAsync(user, newEmail, token);

            if (result.Succeeded)
            {
                return View("EmailConfirmed");
            }

            return View("Error");
        }

    }

}