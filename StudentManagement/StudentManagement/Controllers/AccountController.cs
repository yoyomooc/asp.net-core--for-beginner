using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{


    [AllowAnonymous]
    public class AccountController:Controller   {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {

            return View();

        }


        [HttpGet]
        public IActionResult Fangyu()
        {

            return View();

        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //将数据从RegisterViewModel赋值到ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    City=model.City
                    
                };
                //将用户数据存储在AspNetUsers数据库表中
                var result = await userManager.CreateAsync(user, model.Password);

                //如果成功创建用户，则使用登录服务登录用户信息
                //并重定向到homecontroller的索引操作
                if (result.Succeeded)
                {
                    //如果用户已登录并属于Admin角色。
                    //那么就是Admin正在创建新用户。
                    //所以重定向Admin用户对ListRoles的视图列表

                    if (signInManager.IsSignedIn(User)&&User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers","Admin");

                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index","Home");
                }

                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError(string.Empty, error.Description);


                }

            }                      

            return View(model);

        }

        #region 登录功能

       
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        //防止开放式重定向攻击
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                         //   ModelState.AddModelError(string.Empty, "防止开放式重定向攻击");
                         return RedirectToAction("Index", "home");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "home");
                    }


                   
                }
                ModelState.AddModelError(string.Empty, "登录失败，请重试");
            }
            return View(model);

        }
        #endregion






        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();

            return RedirectToAction("index", "home");


        }


        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {

            var user = await userManager.FindByEmailAsync(email);

            if (user ==null)
            {
                return Json(true);
            }
            else
            {

                return Json($"邮箱：{email}已经被注册使用了。");

            }


        }



    }
}
