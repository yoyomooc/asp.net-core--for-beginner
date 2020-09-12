using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentManagement.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public CanEditOnlyOtherAdminRolesAndClaimsHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {

            // 获取httpContext上下文
            HttpContext httpContext = _httpContextAccessor.HttpContext;


            string loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            string adminIdBeingEdited = _httpContextAccessor.HttpContext.Request.Query["userId"];
            //判断用户是Admin色，并且拥有claim.Type == "Edit Role"且值为true。
            if (context.User.IsInRole("Admin") &&
                context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true"))
            {
                //如果当前拥有admin角色的userid为空，说明进入的是角色列表页面。无须判断当前当前登录用户的id
                if (string.IsNullOrEmpty(adminIdBeingEdited))
                {
                    context.Succeed(requirement);
                }
                else if (adminIdBeingEdited.ToLower() != loggedInAdminId.ToLower())
                {
                    //表示成功满足需求
                    context.Succeed(requirement);
                }
                else
                {
             //       context.Fail();
                }

            }

            return Task.CompletedTask;

        }
    }
}
