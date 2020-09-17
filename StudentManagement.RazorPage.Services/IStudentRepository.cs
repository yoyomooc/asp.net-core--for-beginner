using StudentManagement.RazorPage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.RazorPage.Services
{
     public interface IStudentRepository
    {
                IEnumerable<Student> GetAllStudents();

    }
}
