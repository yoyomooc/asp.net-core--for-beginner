using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
  public  interface IStudentRepository
    {
           
        IEnumerable<Student> GetAllStudents();

        Student Add(Student student);

    }
}
