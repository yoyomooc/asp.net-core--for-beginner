using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {


            var authFilterContext = context.Resource as AuthorizationFilterContext;

          //  如果AuthorizationFilterContext 为NULL，则无法检查是否满足要求，
            //因此我们返回Task.CompletedTask，并且未授权访问。
            if (authFilterContext == null)
            {
                //获取已成功完成的任务。
                return Task.CompletedTask;
            }

            string loggedInAdminId =
            context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            string adminIdBeingEdited = authFilterContext.HttpContext.Request.Query["userId"];

            //如果用户是管理员角色 和 具有“编辑角色”声明类型，声明值为true  
            //并且登录的用户ID与正在编辑的Admin用户的ID不相等
            if (context.User.IsInRole("Admin") &&
            context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") &&
            adminIdBeingEdited.ToLower() != loggedInAdminId.ToLower())
            {
                //Succeed（）方法指定已成功评估需求。
                context.Succeed(requirement);
            }
            else
            {
             //   context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
