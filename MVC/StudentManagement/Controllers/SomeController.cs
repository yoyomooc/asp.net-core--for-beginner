using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class SomeController : Controller
    {
        public string ABC()
        {
            return "ABC操作方法";
        }

        [Authorize(Roles = "Admin")]
        public string XYZ()
        {
            return "XYZ操作方法";
        }

        [AllowAnonymous]
        public string Anyone()
        {
            return "Anyone操作方法";
        }
    }
}
