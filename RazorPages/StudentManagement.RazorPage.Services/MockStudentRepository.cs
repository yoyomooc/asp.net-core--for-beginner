using StudentManagement.RazorPage.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement.RazorPage.Services
{
    public class MockStudentRepository : IStudentRepository
    {
        private List<Student> _studentList;

        public MockStudentRepository()
        {
            _studentList = new List<Student>()
            {
                 new Student() { Id = 1, Name = "张三", ClassName = ClassNameEnum.FirstGrade, Email = "Tony-zhang@52abp.com" },
            new Student() { Id = 2, Name = "李四", ClassName = ClassNameEnum.SecondGrade, Email = "lisi@52abp.com" },
            new Student() { Id = 3, Name = "王二麻子", ClassName = ClassNameEnum.GradeThree, Email = "wang@52abp.com" },
            };
        }

        public Student Add(Student newStudent)
        {
            newStudent.Id= _studentList.Max(s => s.Id) + 1;

            _studentList.Add(newStudent);
            return newStudent;


        }

       
        public IEnumerable<Student> GetAllStudents()
        {
            return _studentList;
        }

        public Student GetStudent(int id)
        {
            return _studentList.FirstOrDefault(e => e.Id == id);
        }

        public Student Update(Student updatedStudent)
        {
            Student student = _studentList
                .FirstOrDefault(e => e.Id == updatedStudent.Id);
            if (student != null)
            {
                student.Name = updatedStudent.Name;
                student.Email = updatedStudent.Email;
                student.ClassName = updatedStudent.ClassName;
                student.PhotoPath = updatedStudent.PhotoPath;
            }
            return student;
        }
    }
}