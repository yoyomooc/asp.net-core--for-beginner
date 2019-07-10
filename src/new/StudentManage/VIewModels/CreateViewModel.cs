using Microsoft.AspNetCore.Http;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.VIewModels
{
    public class CreateViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public ClassNameEnum? ClassName { get; set; }
        public IFormFile Photo { get; set; }
    }
}
