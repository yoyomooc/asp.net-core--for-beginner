using System.Collections.Generic;
using System.Security.Claims;

namespace StudentManagement.Models
{
    public class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
    {
        new Claim("Create Role","创建角色"),
        new Claim("Edit Role","编辑角色"),
        new Claim("Delete Role","删除角色")
    };
    }
}