using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Extentions;
using BaselFinalProjectApp.Infastructure;
using BaselFinalProjectApp.Models.MainModel;
using BaselFinalProjectApp.Models.ViewData;
using BaselFinalProjectApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BaselFinalProjectApp.Controllers
{
    [Authorize(Roles = "User")]
    public class AccountController : BaseController
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
        [AllowAnonymous]
        public IActionResult Login()
        {
            try
            {
                LoginData loginData = new LoginData()
                {
                    LoginModel = new LoginModel(),
                    RegisterModel = new RegisterModel()
                };
                return View(loginData);
            }
            catch (Exception exp)
            {

            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            HttpContext.Session.SetInt32("loginRegister",1);

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _signInManager.SignOutAsync();

                    AppUser appUser = await _userManager
                                        .FindByEmailAsync(loginModel.Email);

                    if (appUser != null)
                    {
                        Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager
                            .PasswordSignInAsync(appUser, loginModel.Password, loginModel.IsRemember, true);

                        if (signInResult.IsLockedOut)
                        {
                            ModelState.AddModelError("", "Sorry. This account is already locked out");
                        }
                        else
                        {
                            if (signInResult.Succeeded)
                            {
                                HttpContext.Session.SetString("UserId", appUser.Id);
                                HttpContext.Session.SetString("UserName", appUser.UserName);

                                IList<string> role = await _userManager.GetRolesAsync(appUser);

                                if (role[0] == "Admin")
                                {
                                    return RedirectToAction("Index", "Account", new { area = "Admin" });
                                }
                                else if (role[0] == "User")
                                {
                                    return RedirectToAction("AccountDashboard", "Account");
                                }
                            }
                            else
                                ModelState.AddModelError("", "Email or Password is incorrect. Please, input correct information");
                        }
                    }
                    else
                        ModelState.AddModelError("","This User is Not Found");
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                }
                return View();
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel, [FromServices] EmailService emailService)
        {
            if (ModelState.IsValid)
            {               
                try
                {
                    AppUser appUser = new AppUser()
                    {
                        UserName = registerModel.Username,
                        Email = registerModel.Email,
                        IsActivate = true,
                    };

                    if (appUser!=null)
                    {
                        IdentityResult identityResult = await _userManager
                                .CreateAsync(appUser, registerModel.Password);

                        await _userManager.AddToRoleAsync(appUser, "User");


                        if (!identityResult.Succeeded)
                            this.SetValidateMessage(identityResult.Errors);
                        else
                        {
                            AppUser user = await _userManager.FindByEmailAsync(registerModel.Email);

                            string confirmationToken = await _userManager
                                    .GenerateEmailConfirmationTokenAsync(user);

                            byte[] encodeToken = Encoding.UTF8.GetBytes(confirmationToken);

                            string codeEcode = WebEncoders.Base64UrlEncode(encodeToken);

                            string confirmationLink = Url.Action("ConfrimEmail",
                            "Account", new
                            {
                                userid = appUser.Id,
                                token = codeEcode
                            },
                            protocol: HttpContext.Request.Scheme);

                            await emailService.SendEmailAsync(registerModel.Email, "Confrim Your Email", confirmationLink);

                            ViewBag.Message = "Please, Check Your Email Address For Confrim Email";

                            return View(nameof(Login));
                        }                            
                    }
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                }
            }
            return View("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult EditEmail()
        {
            return View(new EmailModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmail(EmailModel emailModel, [FromServices] EmailService emailService)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUser appUser = await _userManager
                                .FindByEmailAsync(emailModel.Email);
                    
                    if (appUser != null)
                    {
                        string confirmationToken = await _userManager
                                    .GeneratePasswordResetTokenAsync(appUser);

                        byte[] encodeToken = Encoding.UTF8.GetBytes(confirmationToken);

                        string codeEcode = WebEncoders.Base64UrlEncode(encodeToken);

                        string confirmationLink = Url.Action("ChangePassword",
                        "Account", new
                        {
                            userid = appUser.Id,
                            token = codeEcode
                        },
                        protocol: HttpContext.Request.Scheme);

                        await emailService.SendEmailAsync(appUser.Email, "Reset Your Password", confirmationLink);

                        ViewBag.Message = "Please, Check Your Email Address For Account Password Change";

                        return View(nameof(Login));
                    }
                    else
                        ModelState.AddModelError("", "There is not so User");
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                }
            }

            return View(emailModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(string userid, string token)
        {
            try
            {
                AppUser appUser = await _userManager.FindByIdAsync(userid);

                if (appUser != null)
                {
                    byte[] EncodeToken = WebEncoders.Base64UrlDecode(token);
                    string validatetoken = Encoding.UTF8.GetString(EncodeToken);

                    HttpContext.Session.SetString("token", validatetoken);
                    HttpContext.Session.SetString("passwordUserid", appUser.Id);

                    ViewBag.User = appUser.UserName;
                    return View(new UserPassword());
                }
                else
                {
                    ViewBag.Message = "This User Is Not Found";
                    return View(nameof(Login));
                }
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserPassword userPassword)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Session.GetString("token");
                    string UserId = HttpContext.Session.GetString("passwordUserid");

                    AppUser appUser = await _userManager.FindByIdAsync(UserId);

                    if (appUser != null)
                    {
                        IdentityResult result = await _userManager
                                .ResetPasswordAsync(appUser,token, userPassword.Password);             

                        if (result.Succeeded)
                        {
                            ViewBag.Message = "Your Account Password Is Changed Successfully";

                            return View(nameof(Login));
                        }
                        else
                            this.SetValidateMessage(result.Errors);                   
                    }
                    else
                        ModelState.AddModelError("", "User Account Not Found");
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                }
            }            
            return View(userPassword);
        }

        [HttpGet]
        public IActionResult AccountDashboard()
        {
            try
            {
                string UserId = HttpContext.Session.GetString("UserId");
                string UserName = HttpContext.Session.GetString("UserName");

                DashboardData dashboardData = new DashboardData()
                {
                    AppUserModel = new AppUserModel()
                    {
                        Id = UserId,
                        Username = UserName
                    }
                };

                HttpContext.Session.SetInt32("loginRegister", 0);

                return View(dashboardData);
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfrimEmail(string userid, string token)
        {
            try
            {
                AppUser appUser = await _userManager.FindByIdAsync(userid);

                if (appUser != null)
                {
                    byte[] EncodeToken = WebEncoders.Base64UrlDecode(token);

                    string validatetoken = Encoding.UTF8.GetString(EncodeToken);

                    IdentityResult result = await _userManager
                        .ConfirmEmailAsync(appUser, validatetoken);

                    if (result.Succeeded)
                    {
                        ViewBag.Message = "Email confirmed successfully!";
                    }
                    else
                    {
                        ViewBag.Message = "Error while confirming your email!";
                    }
                }
            }
            catch (Exception)
            {

            }

            return View(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> AccountAddress()
        {
            try
            {
                AppUser appUser = await _baselDb.AppUsers
                                    .Where(z => z.UserName == HttpContext.User.Identity.Name)
                                        .Include(z => z.BillingDetail)
                                            .SingleOrDefaultAsync();

                return View(appUser);
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> AccountDetail()
        {
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    AppUser appUser = await _userManager
                        .FindByNameAsync(HttpContext.User.Identity.Name);

                    AccountViewModel accountViewModel = new AccountViewModel()
                    {
                        FirstName = appUser.Name,
                        LastName = appUser.Lastname,
                        Email = appUser.Email,
                        UserName = appUser.UserName
                    };
                    return View(accountViewModel);
                }
            }
            catch (Exception exp)
            {

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountDetail(AccountViewModel accountViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        AppUser appUser = await _userManager
                            .FindByNameAsync(HttpContext.User.Identity.Name);

                        if (accountViewModel.Password != null && accountViewModel.ConfrimPassword != null && accountViewModel.CurrentPassword != null)
                        {
                            bool userPassword = await _userManager
                            .CheckPasswordAsync(appUser, accountViewModel.CurrentPassword);

                            if (userPassword)
                            {
                                await _userManager.ChangePasswordAsync(appUser, accountViewModel.CurrentPassword, accountViewModel.Password);
                            }
                        }

                        appUser.Name = accountViewModel.FirstName;
                        appUser.Lastname = accountViewModel.LastName;
                        appUser.UserName = accountViewModel.UserName;
                        appUser.Email = accountViewModel.Email;

                        await _signInManager.RefreshSignInAsync(appUser);

                        IdentityResult identityResult = await _userManager.UpdateAsync(appUser);

                        if (identityResult.Succeeded)
                        {
                            await _baselDb.SaveChangesAsync();

                            return View(accountViewModel);
                        }
                        else
                        {
                            foreach (IdentityError item in identityResult.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
            return View(accountViewModel);
        }

        [HttpGet]
        public IActionResult AccountDownload()
        {          
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AccountOrder()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                try
                {
                    AppUser appUser = await _userManager.
                     FindByNameAsync(HttpContext.User.Identity.Name);


                    List<Order> orders = _baselDb.Orders
                        .Where(z => z.AppUserId == appUser.Id)
                            .Include(z => z.ProductOrders)
                                .ToList();

                    return View(orders);
                }
                catch (Exception)
                {
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult AccountWishList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AccountOrderList(int? id)
        {
            if (id!=null)
            {
                try
                {
                    AppUser appUser = await _baselDb.AppUsers
                   .Where(z => z.UserName == HttpContext.User.Identity.Name)
                       .Include(z => z.BillingDetail)
                           .SingleOrDefaultAsync();

                    Order order = await _baselDb.Orders
                        .Where(z => z.AppUserId == appUser.Id)
                            .Where(z => z.Id == id)
                                .Include(z => z.ProductOrders)
                                    .SingleOrDefaultAsync();

                    if (order != null)
                    {
                        OrderCheckOutData orderCheckOutData = new OrderCheckOutData()
                        {
                            AppUser = appUser,
                            Order = order
                        };
                        return View(orderCheckOutData);
                    }
                }
                catch (Exception)
                {
                }
            }
            return RedirectToAction("AccountOrder");
        }

        [HttpGet]
        public async Task<IActionResult> AccountAddressUpdate()
        {
            try
            {
                AppUser appUser = await _baselDb.AppUsers
                    .Where(z => z.UserName == HttpContext.User.Identity.Name)
                        .Include(z => z.BillingDetail)
                            .SingleOrDefaultAsync();

                if (appUser.BillingDetail!=null)
                {
                    OrderModel orderModel = new OrderModel()
                    {
                        Name = appUser.Name,
                        LastName = appUser.Lastname,
                        Apartment = appUser.BillingDetail.Apartment,
                        StreetAddress = appUser.BillingDetail.StreetAddress,
                        Town = appUser.BillingDetail.Town
                    };
                    return View(orderModel);
                }
                else
                {
                    OrderModel orderModel = new OrderModel()
                    {
                        Name = appUser.Name,
                        LastName = appUser.Lastname
                    };
                    return View(orderModel);
                }                
            }
            catch (Exception exp)
            {
                
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountAddressUpdate(OrderModel orderModel)
        {
            if (orderModel!=null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        AppUser appUser = await _baselDb.AppUsers
                            .Where(z => z.UserName == HttpContext.User.Identity.Name)
                                .Include(z => z.BillingDetail)
                                    .SingleOrDefaultAsync();

                        appUser.Name = orderModel.Name;
                        appUser.Lastname = orderModel.LastName;

                        if (appUser.BillingDetail != null)
                        {
                            appUser.BillingDetail.Apartment = orderModel.Apartment;
                            appUser.BillingDetail.StreetAddress = orderModel.StreetAddress;
                            appUser.BillingDetail.Town = orderModel.Town;
                        }
                        else
                        {
                            BillingDetail billingDetail = new BillingDetail()
                            {
                                Town = orderModel.Town,
                                StreetAddress = orderModel.StreetAddress,
                                Apartment = orderModel.Apartment
                            };

                            appUser.BillingDetail = billingDetail;
                        }

                        await _baselDb.SaveChangesAsync();

                        return RedirectToAction("AccountAddress");
                    }
                    catch (Exception)
                    {
                    }
                }
                return View(orderModel);
            }
            return RedirectToAction("AccountAddress");
        }
    }
}
