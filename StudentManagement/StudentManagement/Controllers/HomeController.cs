using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.ViewModels;
using System.Collections.Generic;

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


      
        public IActionResult Index()
        {
         IEnumerable<Student> students = _studentRepository.GetAllStudents(); 

            return View(students);      
            
        }




       
        public IActionResult Details(int? id)
        {
            
            //实例化HomeDetailsViewModel并存储Student详细信息和PageTitle
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = _studentRepository.GetStudent(id??1),
                PageTitle = "学生详细信息"
            };
            return View(homeDetailsViewModel);
        }

       
        public ViewResult Create()
        {
            return View();
        }

    }
}