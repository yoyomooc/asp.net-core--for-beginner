using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger logger;

        //使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository,
            IHostingEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            _studentRepository = studentRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Student> students = _studentRepository.GetAllStudents();

            return View(students);
        }

        public IActionResult Details(int id)
        {
            logger.LogTrace("Trace(跟踪) Log");
            logger.LogDebug("Debug(调试) Log");
            logger.LogInformation("信息(Information) Log");
            logger.LogWarning("警告(Warning) Log");
            logger.LogError("错误(Error) Log");
            logger.LogCritical("严重(Critical) Log");

            //  throw new Exception("在Details视图中抛出异常");

            Student student = _studentRepository.GetStudent(id);
            if (student == null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);
            }

            //实例化HomeDetailsViewModel并存储Student详细信息和PageTitle
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = student,
                PageTitle = "学生详细信息"
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Student student = _studentRepository.GetStudent(id);
            StudentEditViewModel studentEditViewModel = new StudentEditViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                ClassName = student.ClassName,
                ExistingPhotoPath = student.PhotoPath
            };
            return View(studentEditViewModel);
        }

        //通过模型绑定，作为操作方法的参数
        // StudentEditViewModel 会接收来自Post请求的Edit表单数据
        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            //检查提供的数据是否有效，如果没有通过验证，需要重新编辑学生信息
            //这样用户就可以更正并重新提交编辑表单
            if (ModelState.IsValid)
            {
                //从数据库中查询正在编辑的学生信息
                Student student = _studentRepository.GetStudent(model.Id);
                //用模型对象中的数据更新student对象
                student.Name = model.Name;
                student.Email = model.Email;
                student.ClassName = model.ClassName;

                //如果用户想要更改照片，可以上传新照片它会被模型对象上的Photo属性接收
                //如果用户没有上传照片，那么我们会保留现有的照片信息

                if (model.Photo != null)
                {
                    //如果上传了新的照片，则必须显示新的照片信息
                    //因此我们会检查当前学生信息中是否有照片，有的话，就会删除它。
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    //我们将保存新的照片到 wwwroot/images  文件夹中，并且会更新
                    //Student对象中的PhotoPath属性，然后最终都会将它们保存到数据库中
                    student.PhotoPath = ProcessUploadedFile(model);
                }

                //调用仓储服务中的Update方法，保存studnet对象中的数据，更新数据库表中的信息。
                Student updatedstudent = _studentRepository.Update(student);

                return RedirectToAction("index");
            }

            return View(model);
        }

        /// <summary>
        /// 将照片保存到指定路径中，并返回唯一的文件名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string ProcessUploadedFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
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

                if (model.Photo != null)
                {
                    //必须将图像上传到wwwroot中的images文件夹
                    //而要获取wwwroot文件夹的路径，我们需要注入 ASP.NET Core提供的HostingEnvironment服务
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和一个下划线
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images文件夹
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
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