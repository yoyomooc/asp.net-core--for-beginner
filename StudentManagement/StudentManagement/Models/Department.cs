using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ClassNameEnum? ClassName { get; set; }


       
    }
}
