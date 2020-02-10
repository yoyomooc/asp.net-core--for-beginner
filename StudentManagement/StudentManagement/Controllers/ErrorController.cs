using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class ErrorController:Controller
    {
        private ILogger<ErrorController> logger;

        /// <summary>
        /// 通过ASP.Net Core 依赖注入服务注入ILogger服务
        /// 将指定类型的控制器作为泛型参数
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;

        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {

            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {

                case 404:

                    ViewBag.ErrorMessage = "抱歉，您访问的页面不存在";

     logger.LogWarning($"发生了一个404错误。路径={statusCodeResult.OriginalPath}以及查询字符串={statusCodeResult.OriginalQueryString}");

                    //ViewBag.Path = statusCodeResult.OriginalPath;
                    //ViewBag.QueryStr = statusCodeResult.OriginalQueryString;
                    //ViewBag.BasePath = statusCodeResult.OriginalPathBase;

                    break;

            }

            return View("NotFound");

        }


        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
         var exceptionHandlerPathFeature=   HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            logger.LogError($"路径:{exceptionHandlerPathFeature.Path},产生了一个错误{exceptionHandlerPathFeature.Error}");


            //ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            //ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            //ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;


            return View("Error");

        }


    }
}
