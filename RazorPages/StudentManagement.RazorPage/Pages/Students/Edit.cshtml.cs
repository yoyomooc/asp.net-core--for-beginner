using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagement.RazorPage.Models;
using StudentManagement.RazorPage.Services;
using System;
using System.IO;

namespace StudentManagement.RazorPage.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly IStudentRepository studentRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EditModel(IStudentRepository studentRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this.studentRepository = studentRepository;
            //使用 IWebHostEnvironment 服务，我们可以获取wwwroot文件夹下的路径地址信息
            this.webHostEnvironment = webHostEnvironment;
        }

        //这是显示模板将用于的属性,显示现有的学生数据
        [BindProperty]
        public Student Student { get;    set; }

        // 我们使用这个属性来存储和处理新上传的照片
        [BindProperty]
        public IFormFile Photo { get; set; }

        [BindProperty]
        public bool Notify { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// 设置id为可选参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult OnGet(int? id)
        {
            //如果id参数有值，查询现有值学生详细信息，否则创建一个新学生

            if (id.HasValue)
            {
                Student = studentRepository.GetStudent(id.Value);
            }
            else
            {
                Student = new Student();
            }
            if (Student == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    //上传新照片的时候，需要检查当前学生是否有已经存在的照片，如果有的话，就需要删除它，再上传新照片。
                    if (Student.PhotoPath != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                            "images", Student.PhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    // 将新照片保存到wwwroot/images文件夹中，并更新student 的PhotoPath属性
                    Student.PhotoPath = ProcessUploadedFile();
                }

                //如果学生ID为> 0，则调用Update()来更新现有学生的详细信息，否则调用Add()来添加新学生
                if (Student.Id > 0)
                {
                    Student = studentRepository.Update(Student);
                }
                else
                {
                    Student = studentRepository.Add(Student);
                }

                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPostUpdateNotificationPreferences(int id)
        {
            if (Notify)
            {
                Message = "您已经打开了消息通知功能";
            }
            else
            {
                Message = "你已经关闭了消息通知功能";
            }

            //将确认消息存储在TempData中
            TempData["message"] = Message;
            // 将请求重定向到Details razor页面，并传递StudentId和Message。
            //StudentId作为路由参数传递 ， 将Message作为查询字符串传递
            return RedirectToPage("Details", new { id = id });
        }

        /// <summary>
        /// 处理文件上传
        /// </summary>
        /// <returns></returns>
        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;

            if (Photo != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");

                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}