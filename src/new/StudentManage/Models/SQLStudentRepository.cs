using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<SQLStudentRepository> logger;

        public SQLStudentRepository(AppDbContext context,
            ILogger<SQLStudentRepository> logger)
        {
            this.context = context;
            this.logger = logger;
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

            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
            }

            return student;

        }

        public IEnumerable<Student> GetAllStudents()
        {
            logger.LogTrace("Trace(跟踪) Log");
            logger.LogDebug("Debug(调试) Log");
            logger.LogInformation("信息(Information) Log");
            logger.LogWarning("警告(Warning) Log");
            logger.LogError("错误(Error) Log");
            logger.LogCritical("严重(Critical) Log");
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
