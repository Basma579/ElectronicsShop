using ElectronicsShop.Dtos;
using ElectronicsShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectronicsShop.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> _userManage,   SignInManager<IdentityUser> signInManager , RoleManager<IdentityRole> roleManager)
        {

              userManager = _userManage;
            _signInManager = signInManager;
            _roleManager = roleManager;


        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }


   

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    LockoutEnabled = false

                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    bool adminRoleExists = await _roleManager.RoleExistsAsync("User");
                    if (!adminRoleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User"));
                    }
                    await userManager.AddToRoleAsync(user, "User");

                    await _signInManager.PasswordSignInAsync(user.UserName,
                                                             model.Password, false, false);
                    return RedirectToAction("ViewCartItems", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return View(model);
        }



        public async Task<IActionResult> Login(UserLoginDto model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

              

                var signedUser = await userManager.FindByEmailAsync(model.Email);
                if (signedUser != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(signedUser.UserName,
                    model.Password, model.RememberMe, false);



                    if (result.Succeeded)
                    {

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {

                            return Redirect(returnUrl);
                        }
                        else
                        {
                           
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Data");
            }

            return View(model);
        }



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}


                
