using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace StudentManagement.Controllers
{


    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly HostingEnvironment hostingEnvironment;

        //使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository, HostingEnvironment hostingEnvironment)
        {
            _studentRepository = studentRepository;
            this.hostingEnvironment = hostingEnvironment;

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
                Student = _studentRepository.GetStudent(id ?? 1),
                PageTitle = "学生详细信息"
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {

                string uniqueFileName = null;

                if (model.Photos!=null&&model.Photos.Count>0)
                {
                    foreach (var photo in model.Photos)
                    {
 //必须将图像上传到wwwroot中的images文件夹
 //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的HostingEnvironment服务
 //通过HostingEnvironment服务去获取wwwroot文件夹的路径
 string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
 //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线

 uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
 string filePath = Path.Combine(uploadsFolder, uniqueFileName);
 //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images文件夹
 photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }


                   

                }
                       Student newStudent = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    ClassName = model.ClassName,
                    PhotoPath = uniqueFileName
                };

            _studentRepository.Add(newStudent);
                return RedirectToAction("Details", new { id = newStudent.Id });
            }
            return View();
        }
    }
}