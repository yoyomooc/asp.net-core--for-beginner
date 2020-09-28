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
            newStudent.Id = _studentList.Max(s => s.Id) + 1;

            _studentList.Add(newStudent);
            return newStudent;
        }

        public Student Delete(int id)
        {
            var studentToDelete = _studentList.FirstOrDefault(e => e.Id == id);

            if (studentToDelete != null)
            {
                _studentList.Remove(studentToDelete);
            }

            return studentToDelete;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _studentList;
        }

        public Student GetStudent(int id)
        {
            return _studentList.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<ClassHeadCount> StudentCountByClassNameEnum(ClassNameEnum? className)
        {
            IEnumerable<Student> query = _studentList;

            if (className.HasValue)
            {
                query = query.Where(e => e.ClassName == className.Value);
            }

            return query.GroupBy(e => e.ClassName)
                                 .Select(g => new ClassHeadCount()
                                 {
                                     ClassName = g.Key.Value,
                                     Count = g.Count()
                                 }).ToList();
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

        public IEnumerable<Student> Search(string searchTerm = null)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return _studentList;
            }

            return _studentList.Where(e => e.Name.Contains(searchTerm) ||
                                            e.Email.Contains(searchTerm)).ToList();
        }
    }
}