using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagement.RazorPage.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement.RazorPage.Services
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly AppDbContext context;

        public SQLStudentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Student Add(Student newStudent)
        {
            context.Students.Add(newStudent);
            context.SaveChanges();
            return newStudent;
        }

        public Student Delete(int id)
        {
            Student student = context.Students.Find(id);
            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
            }
            return student;
        }

        public IEnumerable<Student> GetAllStudents()
        {
    return context.Students
                    .FromSqlRaw<Student>("SELECT * FROM Students")
                    .ToList();        }

        public Student GetStudent(int id)
        {
                SqlParameter parameter = new SqlParameter("@Id", id);


            return context.Students
                        .FromSqlRaw<Student>("spGetStudentById {0}", parameter)
                        .ToList()
                        .FirstOrDefault();
        }

        public IEnumerable<Student> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return context.Students;
            }

            return context.Students.Where(e => e.Name.Contains(searchTerm) ||
                                            e.Email.Contains(searchTerm));
        }

        public IEnumerable<ClassHeadCount> StudentCountByClassNameEnum(ClassNameEnum? className)
        {
            IEnumerable<Student> query = context.Students;
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
            var student = context.Students.Attach(updatedStudent);
            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return updatedStudent;
        }
    }
}