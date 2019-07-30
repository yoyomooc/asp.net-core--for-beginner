using Microsoft.AspNetCore.Http;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class StudentCreateViewModel
    {
      

        [Required(ErrorMessage = "请输入名字"), MaxLength(50, ErrorMessage = "名字的长度不能超过50个字符")]
        [Display(Name = "名字")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "班级信息")]
        public ClassNameEnum? ClassName { get; set; }

        [Display(Name = "电子邮件")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "邮箱的格式不正确")]
        [Required(ErrorMessage = "请输入邮箱地址")]
        public string Email { get; set; }

        [Display(Name="图片")]
        public List<IFormFile> Photos { get; set; }

    }
}
