using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class StudentController:Controller
    {
        private IStudentRepository _studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public IActionResult Details(int id)
        {
            Student model = _studentRepository.GetStudent(id);
            return View(model);
        }
    }
}
