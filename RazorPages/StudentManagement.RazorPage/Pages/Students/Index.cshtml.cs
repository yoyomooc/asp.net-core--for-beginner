using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagement.RazorPage.Models;
using StudentManagement.RazorPage.Services;
using System.Collections.Generic;

namespace StudentManagement.RazorPage.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly IStudentRepository studentRepository;
        
        
        /// <summary>
        /// //这个公共属性保存学生列表 显示模板(Index.html)可以访问此属性
        /// </summary>
        public IEnumerable<Student> Students { get; set; }


        /// <summary>
        /// 注册IStudentRepository服务。通过这项服务知道如何查询学生列表
        /// </summary>
        /// <param name="studentRepository"></param>
        public IndexModel(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        /// <summary>
        /// 此方法处理发送GET请求 到路由 /Students/Index  
        /// </summary>
        public void OnGet()
        {
            Students = studentRepository.GetAllStudents();
        }
    }
}