using StudentManagement.RazorPage.Models;
using System.Collections.Generic;

namespace StudentManagement.RazorPage.Services
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAllStudents();

        Student GetStudent(int id);

        Student Update(Student updatedStudent);

        Student Add(Student newStudent);

        Student Delete(int id);

    }
}