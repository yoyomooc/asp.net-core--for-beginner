using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace StudentManagement.Controllers
{

    
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        //使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository, 
            IHostingEnvironment hostingEnvironment)
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
                Student = _studentRepository.GetStudent(id??1),
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

                // 如果传入模型对象中的Photo属性不为null,并且Count>0，则表示用户选择至少一个要上传的文件。
                if (model.Photos != null && model.Photos.Count > 0)
                {
                    //循环每个选定的文件
                    foreach (IFormFile photo in model.Photos)
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
                    //   将文件名保存在student对象的PhotoPath属性中 它将保存到数据库 Students的 表中
                    PhotoPath = uniqueFileName
                };

                _studentRepository.Add(newStudent);
                return RedirectToAction("details", new { id = newStudent.Id });
            }

            return View();
        }

    }
}