using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagement.RazorPage.Models;
using StudentManagement.RazorPage.Services;

namespace StudentManagement.RazorPage.Pages.Students
{
    public class DetailsModel : PageModel
    {
        private readonly IStudentRepository studentRepository;

        public DetailsModel(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }


        public Student Student { get; private set; }

       
        /// <summary>
        /// //模型绑定自动映射查询字符串id的值，映射到OnGet()方法上的设置为id参数
        /// </summary>
        /// <param name="id"></param>
        public IActionResult OnGet(int id)
        {
            //Id = id;

            Student = studentRepository.GetStudent(id);

             if(Student == null)
    {
        return RedirectToPage("/NotFound");
    }
                 return Page();

        }
    }
}