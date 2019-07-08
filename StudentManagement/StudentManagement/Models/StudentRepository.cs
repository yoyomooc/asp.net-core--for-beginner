using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class StudentRepository : IStudentRepository
    {
        public Student Add(Student student)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public Student GetStudent(int id)
        {
            // 写逻辑实现 查询学生详情信息
            throw new NotImplementedException();
        }

        public void Save(Student student)
        {
            // 写逻辑实现保存学生信息
            throw new NotImplementedException();
        }
    }
}
