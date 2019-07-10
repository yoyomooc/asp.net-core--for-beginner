using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{

    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public ClassNameEnum? ClassName { get; set; }


        public string PhotoPath { get; set; }
    }
}