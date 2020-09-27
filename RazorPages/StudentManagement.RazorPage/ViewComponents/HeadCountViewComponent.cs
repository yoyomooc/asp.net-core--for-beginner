using Microsoft.AspNetCore.Mvc;
using StudentManagement.RazorPage.Models;
using StudentManagement.RazorPage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.RazorPage.ViewComponents
{
    public class HeadCountViewComponent : ViewComponent
    {
        private readonly IStudentRepository studentRepository;

        public HeadCountViewComponent(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public IViewComponentResult Invoke(ClassNameEnum? className = null)
        {
            var result = studentRepository.StudentCountByClassNameEnum(className);
            return View(result);
        }
    }
}
