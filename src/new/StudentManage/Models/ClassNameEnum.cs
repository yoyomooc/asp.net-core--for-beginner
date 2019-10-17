using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public enum ClassNameEnum
    {
        [Display(Name = "未分配")]
        None,
        [Display(Name = "一年级")]
        FirstGrade,
        [Display(Name = "二年级")]
        SecondGrade,
        [Display(Name = "三年级")]
        GradeThree
    }
}
