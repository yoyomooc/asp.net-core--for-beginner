using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.RazorPage.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ClassNameEnum? ClassName { get; set; }
        public string Email { get; set; }
        public string PhotoPath { get; set; }

    }
}
