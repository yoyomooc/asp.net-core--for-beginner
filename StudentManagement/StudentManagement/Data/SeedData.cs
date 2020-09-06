using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Data
{
    public static class SeedData
    {
        public static IApplicationBuilder UseDataInitializer(
           this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetService<AppDbContext>();

                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                System.Console.WriteLine("开始执行迁移数据库...");

                dbcontext.Database.Migrate();
                System.Console.WriteLine("数据库迁移完成...");

                #region 初始化学生数据

                if (!dbcontext.Students.Any())
                {
                    System.Console.WriteLine("开始创建种子数据中...");

                    dbcontext.Students.Add(new Student
                    {
                        Id = 1,
                        Name = "梁桐铭",
                        ClassName = ClassNameEnum.FirstGrade,
                        Email = "ltm@ddxc.org",
                    });
                    dbcontext.Students.Add(new Student
                    {
                        Id = 2,
                        Name = "角落的白板报",
                        ClassName = ClassNameEnum.GradeThree,
                        Email = "werltm@qq.com",
                    });
                }

                #endregion 初始化学生数据


                #region 初始化用户

                var adminUser = dbcontext.Users.FirstOrDefault(a => a.UserName == "ltm@ddxc.org");

                if (adminUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = "ltm@ddxc.org",
                        Email = "ltm@ddxc.org",
                        EmailConfirmed = true,
                        City = "成都",                       
                    };

                    var identityResult = userManager.CreateAsync(user, "bb123456").GetAwaiter().GetResult();

                    var role = dbcontext.Roles.Add(new IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    });

                    dbcontext.SaveChanges();

                    dbcontext.UserRoles.Add(new IdentityUserRole<string>
                    {
                        RoleId = role.Entity.Id,
                        UserId = user.Id
                    });

                    var userclaims = new List<IdentityUserClaim<string>>();

                    userclaims.Add(new IdentityUserClaim<string>
                    {
                        UserId = user.Id,
                        ClaimType = "Create Role",
                        ClaimValue = "Create Role"

                    });
                    userclaims.Add(new IdentityUserClaim<string>
                    {
                        UserId = user.Id,
                        ClaimType = "Edit Role",
                        ClaimValue = "Edit Role"

                    });
                    userclaims.Add(new IdentityUserClaim<string>
                    {
                        UserId = user.Id,
                        ClaimType = "Delete Role",
                        ClaimValue = "Delete Role"

                    });
                    
                    userclaims.Add(new IdentityUserClaim<string>
                    {
                        UserId = user.Id,
                        ClaimType = "EditStudent",
                        ClaimValue = "EditStudent"

                    });
                    dbcontext.UserClaims.AddRange(userclaims);

                    dbcontext.SaveChanges();





               

#endregion 初始化用户
            }


            else
            {
                System.Console.WriteLine("无需创建种子数据...");
            }
        
        return builder;
            }
        }
    }
}

