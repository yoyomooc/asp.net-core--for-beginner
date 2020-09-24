using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly ILogger logger;
        private readonly  AppDbContext context;

        public SQLStudentRepository(AppDbContext context,ILogger<SQLStudentRepository> logger)
        {
            this.logger = logger;
            this.context = context;
        }


        public Student Add(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
            return student;
        }

        public Student Delete(int id)
        {
            Student student = context.Students.Find(id);
            if (student!=null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
            }
            return student;

        }

        public IEnumerable<Student> GetAllStudents()
        {
            logger.LogTrace("学生信息 Trace(跟踪) Log");
            logger.LogDebug("学生信息 Debug(调试) Log");
            logger.LogInformation("学生信息 信息(Information) Log");
            logger.LogWarning("学生信息 警告(Warning) Log");
            logger.LogError("学生信息 错误(Error) Log");
            logger.LogCritical("学生信息 严重(Critical) Log");


            return context.Students;
        }

        public Student GetStudent(int id)
        {
            return context.Students.Find(id);
        }

        public Student Update(Student updateStudent)
        {

            var student = context.Students.Attach(updateStudent);

            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            context.SaveChanges();

            return updateStudent;

        }
    }
}
