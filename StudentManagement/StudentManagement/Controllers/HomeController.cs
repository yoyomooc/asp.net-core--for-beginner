using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using StudentManagement.Models;
using StudentManagement.Security.CustomTokenProvider;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StudentManagement.Controllers
{

    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ILogger logger;

        private readonly IDataProtector dataProtector;

        //使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository, IWebHostEnvironment hostingEnvironment,
            ILogger<HomeController> logger,DataProtectionPurposeStrings dataProtectionPurposeStrings,IDataProtectionProvider dataProtectionProvider)
        {
            _studentRepository = studentRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
            dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.StudentIdRouteValue);
            
        }



        public IActionResult Index()
        {
            IEnumerable<Student> students = _studentRepository.GetAllStudents().
                Select(s=>{
                //加密ID值并存储在EncryptedId属性中
                s.EncryptedId = dataProtector.Protect(s.Id.ToString());
                return s;            
            });

            return View(students);

        }




        // Details视图接收加密后的StudentID
        public IActionResult Details(string id)
        {

            //logger.LogTrace("Trace(跟踪) Log");
            //logger.LogDebug("Debug(调试) Log");
            //logger.LogInformation("信息(Information) Log");
            //logger.LogWarning("警告(Warning) Log");
            //logger.LogError("错误(Error) Log");
            //logger.LogCritical("严重(Critical) Log");
            //  throw new Exception("此异常发生在Details视图中");

            //使用 Unprotect()方法来解析学生id
            var decryptedId =   dataProtector.Unprotect(id);

           


         var decryptedStudentId= Convert.ToInt32(decryptedId);


            Student student = _studentRepository.GetStudent(decryptedStudentId);

            if (student == null)
            {
                ViewBag.ErrorMessage = $"学生Id={id}的信息不存在，请重试。";
                return View("NotFound");
            }

            //实例化HomeDetailsViewModel并存储Student详细信息和PageTitle
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = student,
                PageTitle = "学生详细信息"
            };
            //将ViewModel对象传递给View()方法
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
                    uniqueFileName = ProcessUploadedFile(model);

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


        //1、视图
        //视图模型
        //2、对应的页面调整

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Student student = _studentRepository.GetStudent(id);
         

                StudentEditVidewModel studentEditVidew = new StudentEditVidewModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    ClassName = student.ClassName,
                    ExistingPhotoPath = student.PhotoPath
                };

                return View(studentEditVidew);         

         //   throw new Exception("查询不到这个学生信息");       


        }

        [HttpPost]
public  IActionResult Edit(StudentEditVidewModel model)
        {

            //检查提供的数据是否有效，如果没有通过验证，需要重新编辑学生信息
            //这样用户就可以更正并重新提交编辑表单
            if (ModelState.IsValid)
            {                
                Student student = _studentRepository.GetStudent(model.Id);
                student.Email = model.Email;
                student.Name = model.Name;
                student.ClassName = model.ClassName;

                if (model.Photos.Count>0)
                {

                    if (model.ExistingPhotoPath!=null)
                    {
 string filePahth = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);

                        System.IO.File.Delete(filePahth);

                    }
                                   
                    student.PhotoPath = ProcessUploadedFile(model);

                }



          Student updateStudent=      _studentRepository.Update(student);


                return RedirectToAction("Index");

            }

            return View(model);


          
        }



        /// <summary>
        /// 将照片保存到指定的路径中，并返回唯一的文件名
        /// </summary>
        /// <returns></returns>
        private string ProcessUploadedFile(StudentCreateViewModel model)
        {
            
            
            string uniqueFileName = null;

            if (model.Photos.Count > 0)
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

                    //因为使用了非托管资源，所以需要手动进行释放
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/images文件夹
                        photo.CopyTo(fileStream);
                    }


                  
                }
            }


            

            return uniqueFileName;

        }




    }
}