using System.Collections.Generic;

namespace StudentManagement.Models
{
    public interface IStudentRepository
    {
        /// <summary>
        /// 通过 Id 来获取学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Student GetStudent(int id);
        /// <summary>
        /// 获取所有的学生信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<Student> GetAllStudents();
        /// <summary>
        /// 添加一名新的学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Student Add(Student student);
        /// <summary>
        /// 更新一名学生的信息
        /// </summary>
        /// <param name="updateStudent"></param>
        /// <returns></returns>
        Student Update(Student updateStudent);
        /// <summary>
        /// 删除一名学生的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Student Delete(int id);



    }
}
