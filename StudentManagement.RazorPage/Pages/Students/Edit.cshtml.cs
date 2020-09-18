using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagement.RazorPage.Models;
using StudentManagement.RazorPage.Services;

namespace StudentManagement.RazorPage.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly IStudentRepository studentRepository;

        public EditModel(IStudentRepository studentRepository)
        {
          this.studentRepository=studentRepository;
        }

            //这是显示模板将用于的属性,显示现有的学生数据
        public Student Student { get; private set; }

         
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
    Student = studentRepository.Update(student);
    return RedirectToPage("Index");
}
    }
}
