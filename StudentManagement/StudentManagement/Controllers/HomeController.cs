using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        //使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
                                 
        }

        public string Index()
        {            //返回学生的名字
            return _studentRepository.GetStudent(1).Name;                       
            
        }


        public IActionResult Details()
        {
            Student model = _studentRepository.GetStudent(1);
            return View(model);

        }




    }
}