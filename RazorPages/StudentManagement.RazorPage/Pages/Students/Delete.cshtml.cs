using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagement.RazorPage.Models;
using StudentManagement.RazorPage.Services;

namespace StudentManagement.RazorPage.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly IStudentRepository studentRepository;

        public DeleteModel(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        [BindProperty]
        public Student Student { get; set; }

        public IActionResult OnGet(int id)
        {
            Student = studentRepository.GetStudent(id);

            if (Student == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            Student deletedStudent = studentRepository.Delete(Student.Id);

            if (deletedStudent == null)
            {
                return RedirectToPage("/NotFound");
            }

            return RedirectToPage("Index");
        }
    }
}