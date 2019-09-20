using Microsoft.AspNetCore.Mvc;
using StudentManagement.CustomerUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "邮箱地址")]
        [Remote(action:"IsEmailInUse",controller:"Account")]
        [ValidEmailDomain(allowedDomain:"52abp.com",ErrorMessage ="电子邮件后缀必须是52abp.com")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name ="确认密码")]
        [Compare("Password",ErrorMessage ="密码与确认密码不一致，请重新输入")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "城市")]
        public string City { get; set; }


    }
}
