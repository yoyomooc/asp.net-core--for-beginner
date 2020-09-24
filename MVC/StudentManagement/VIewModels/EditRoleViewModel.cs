using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class EditRoleViewModel
    {

        public EditRoleViewModel()
        {
            Users = new List<string>();
        }


        [Display(Name ="角度Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }


        public List<string> Users { get; set; }


    }
}
