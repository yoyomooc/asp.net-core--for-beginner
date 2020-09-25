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
        public Student Student { get; private set; }

        // 我们使用这个属性来存储和处理新上传的照片
        [BindProperty]
        public IFormFile Photo { get; set; }


        [BindProperty]
        public bool Notify { get; set; }

        public string Message { get; set; }


        public IActionResult OnGet(int id)
        {
            Student = studentRepository.GetStudent(id);

            if (Student == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public IActionResult OnPost(Student student)
        {
            if (Photo != null)
            {
                //上传新照片的时候，需要检查当前学生是否有已经存在的照片，如果有的话，就需要删除它，再上传新照片。
                if (student.PhotoPath != null)
                {
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                        "images", student.PhotoPath);
                    System.IO.File.Delete(filePath);
                }
                // 将新照片保存到wwwroot/images文件夹中，并更新student 的PhotoPath属性
                student.PhotoPath = ProcessUploadedFile();
            }
            Student = studentRepository.Update(student);
            return RedirectToPage("Index");
        }

        public void OnPostUpdateNotificationPreferences(int id)
        {
            if (Notify)
            {
                Message = "您已经打开了消息通知功能";
            }
            else
            {
                Message = "你已经关闭了消息通知功能";
            }

            Student = studentRepository.GetStudent(id);
        }


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