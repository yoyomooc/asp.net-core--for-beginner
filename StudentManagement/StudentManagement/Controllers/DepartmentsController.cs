using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

        public string Details()
        {
            return "DepartmentsController中的Details() 方法。";
        }
    }
}
