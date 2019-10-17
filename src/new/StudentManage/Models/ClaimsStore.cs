using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
    {
        new Claim("创建角色", "Create Role"),
        new Claim("编辑角色","Edit Role"),
        new Claim("删除角色","Delete Role")
    };
    }
}
