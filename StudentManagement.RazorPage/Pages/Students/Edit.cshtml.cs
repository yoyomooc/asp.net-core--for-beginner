using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [BindProperty]
public Student Student { get; set; }

        public IActionResult OnPost(Student student)
{
    Student = studentRepository.Update(student);
    return RedirectToPage("Index");
}
    }
}
