using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class StudentRepository : IStudentRepository
    {
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
