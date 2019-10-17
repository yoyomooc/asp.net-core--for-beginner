using System.ComponentModel.DataAnnotations;

namespace StudentManagement.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "邮箱地址为必填项")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    }
}
