using System.Collections.Generic;

namespace StudentManagement.Models
{
    public interface IStudentRepository
    {

        Student GetStudent(int id);


        IEnumerable<Student> GetAllStudents();

        Student Add(Student student);


        Student Update(Student updateStudent);

        Student Delete(int id);



    }
}
