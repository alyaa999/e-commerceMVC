using AutoMapper;
using e_commerce.Application.Common.Interfaces;
using e_commerce.Domain.Entites;
using e_commerce.Infrastructure.Entites;
using e_commerce.Infrastructure.Repository;
using e_commerce.Web.Models;
using e_commerce.Web.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace e_commerce.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ECommerceDBContext _context;
        public UserManager<ApplicationUser> UserManager { get; } //RepoService layer for user 
        public SignInManager<ApplicationUser> SignInManager { get; }
        public IMapper _mapper { get; }

        private readonly IEmailSenderService _emailSender;
        private readonly IHomeRepository homeRepository;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ECommerceDBContext context, IEmailSenderService emailSender , IHomeRepository homeRepository,IMapper mapper)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _context = context;

            _emailSender = emailSender;
            this.homeRepository = homeRepository;
            _mapper = mapper;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var DbCategories = homeRepository.GetCategories();

            var categories = _mapper.Map<List<CategoryViewModel>>(DbCategories?.ToList() ?? new List<Category>());
            ViewBag.Categories = categories;
            base.OnActionExecuting(filterContext);
        }



        //<!-- ✅  Register Form -->
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel newUserVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser usermodel = new ApplicationUser();
                usermodel.Email = newUserVM.Email;
                usermodel.UserName = newUserVM.UserName;
                usermodel.FirstName = newUserVM.FirstName;
                usermodel.LastName = newUserVM.LastName;
                usermodel.PasswordHash = newUserVM.Password;

                IdentityResult result = await UserManager.CreateAsync(usermodel, newUserVM.Password);

                if (result.Succeeded)
                {
                    // ✅ Assign Role
                    await UserManager.AddToRoleAsync(usermodel, newUserVM.Role);

                    // ✅ Add to Customer or Seller table
                    if (newUserVM.Role == "Customer")
                    {
                        var customer = new Customer { ApplicationUserId = usermodel.Id };
                        _context.Customers.Add(customer);
                    }
                    else if (newUserVM.Role == "Seller")
                    {
                        var seller = new Seller { ApplicationUserId = usermodel.Id };
                        _context.Sellers.Add(seller);
                    }

                    await _context.SaveChangesAsync();

                    // ✅ Send Email Confirmation Link
                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(usermodel);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userId = usermodel.Id,
                        token
                    }, protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(usermodel.Email, "Confirm your email",
                        $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.");

                    // ✅ Show instruction to check email
                    return View("CheckYourEmail");
                }
                else
                {
                    foreach (var errorItem in result.Errors)
                    {
                        ModelState.AddModelError("Element", errorItem.Description);
                    }
                }
            }
            return View(newUserVM);
        }





        //<!-- ✅ Login Form -->
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel UserVM)
        {
            if (ModelState.IsValid)
            {
                //check db
                ApplicationUser UserFromDB = await UserManager.FindByEmailAsync(UserVM.Email);

                if (UserFromDB != null)
                {
                    bool found = await UserManager.CheckPasswordAsync(UserFromDB, UserVM.Password);

                    if (found)
                    {
                        // ✅ check if the email is confirmed
                        if (!await UserManager.IsEmailConfirmedAsync(UserFromDB))
                        {
                            ModelState.AddModelError("", "You must confirm your email before logging in.");
                            return View(UserVM);
                        }

                        await SignInManager.SignInAsync(UserFromDB, isPersistent: UserVM.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Wrong Email or Password!");
            return View(UserVM);
        }



        //<!-- ✅ ExternalLogin -->
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction("Login");
            }

            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            var signInResult = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            // If the user does not have an account, we can register them
            var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);

            if (email != null)
            {
                var user = new ApplicationUser
                {
                    UserName = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.GivenName) ?? "First",
                    Email = email,
                    FirstName = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.GivenName) ?? "First",
                    LastName = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Surname) ?? "Last",
                    EmailConfirmed = true
                };

                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await UserManager.AddLoginAsync(user, info);

                    // 👉 Store user temporarily
                    TempData["ExternalUserId"] = user.Id;
                    return RedirectToAction("RoleModal");
                }
            }

            // Show error or fallback
            return RedirectToAction("Login");
        }



        public IActionResult RoleModal()
        {
            ViewBag.UserId = TempData["ExternalUserId"];
            return View();
        }



        [HttpGet]
        public IActionResult ChooseRole()
        {
            return View(); // return a view with buttons for "Customer" and "Seller"
        }

        [HttpPost]
        public async Task<IActionResult> ChooseRole(string role, string userId)
        {
            if (role != "Customer" && role != "Seller")
                return RedirectToAction("Login");

            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("Login");

            await UserManager.AddToRoleAsync(user, role);

            if (role == "Customer")
                _context.Customers.Add(new Customer { ApplicationUserId = user.Id });
            else if (role == "Seller")
                _context.Sellers.Add(new Seller { ApplicationUserId = user.Id });

            await _context.SaveChangesAsync();
            await SignInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return View("Error");
            }

            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            var result = await UserManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("EmailConfirmed");
            }

            return View("Error");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Please enter your email.");
                return View();
            }

            var user = await UserManager.FindByEmailAsync(email);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user)))
            {
                // علشان منقولش إذا كان الإيميل موجود ولا لأ (حماية)
                return View("ForgotPasswordConfirmation");
            }

            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { userId = user.Id, token }, protocol: HttpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(email, "Reset Password",
                $"Click <a href='{resetLink}'>here</a> to reset your password.");

            return View("ForgotPasswordConfirmation");
        }



        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return View("Error");
            }

            var model = new ResetPasswordViewModel { UserId = userId, Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null)
                return View("Error");

            var result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }



        //<!-- ✅ Logout -->
        public IActionResult Logout()
        {
            SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

 
    }
}
