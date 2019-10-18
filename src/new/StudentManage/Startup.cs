using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Middleware;
using StudentManagement.Models;
using StudentManagement.Security;
using System;

namespace StudentManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
       options => options.UseSqlServer(_config.GetConnectionString("StudentDBConnection")));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Admin/AccessDenied";
                //   options.Cookie.Name = "YourAppCookieName";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //   options.LoginPath = "/Identity/Account/Login";
                // ReturnUrlParameter requires
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role"));
                 options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));


                options.AddPolicy("EditRolePolicy",policy =>
            policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                //确认失败后,是否允许调用身份验证处理程序。默认为true。
                //例如可能正在记录日志。设置为false后，就不会允许了。
                //   options.InvokeHandlersAfterFailure = false;


                //options.AddPolicy("EditRolePolicy", 
                //    policy => policy.RequireAssertion(context => AuthorizeAccess(context)));

                //options.AddPolicy("EditRolePolicy", 
                //    policy => policy.RequireClaim("Edit Role","true"));



                //扩展展示：为了满足以下政策，已登录用户的Conutry的值带有 "USA", "India", "UK"之一即可。不要求满足所有
                options.AddPolicy("AllowedCountryPolicy",
       policy => policy.RequireClaim("Country", "USA", "India", "UK"));

            });


            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddScoped<IStudentRepository, SQLStudentRepository>();

            //注册第一个处理程序
            services.AddSingleton<IAuthorizationHandler,CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            //注册第二个处理程序
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            //以上实现了在asp.net core中的多个自定义授权处理程序

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //如果环境是Development，调用 Developer Exception Page
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }


        #region 扩展的私有方法

        /// <summary>
        /// 判断授权访问(私有封装方法)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            var result= context.User.IsInRole("Admin") &&
                    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                    context.User.IsInRole("Super Admin");


            return result;
        }


        #endregion


    }
}