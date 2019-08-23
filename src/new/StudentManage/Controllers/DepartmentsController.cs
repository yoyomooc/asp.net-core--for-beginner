using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagement.Controllers
{

    
    public class DepartmentsController: Controller
    {
        public string List()
        {
            if (Debugger.IsAttached)
            {

                Environment.Exit(0);

                return "IsAttached() 方法。";

            }

           


           

            return "DepartmentsController中的List() 方法。";
        }

        public ViewResult Details(int? id)
        {
            throw new Exception("在Details视图中抛出异常");

            // 其他代码
        }

        public JsonResult ReturnJson()
        {
            var model = new StudentManagement.Models.Student();


            return Json(model);
        }

     
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
                                  

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
