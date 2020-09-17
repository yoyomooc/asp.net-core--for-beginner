using StudentManagement.RazorPage.Models;
using System;
using System.Collections.Generic;
using System.Text;

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

        public IEnumerable<Student> GetAllStudents()
        {
            return _studentList;
        }
    } }
