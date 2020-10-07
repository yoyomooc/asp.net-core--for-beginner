using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentManagement.Data;
using StudentManagement.Middlewares;
using StudentManagement.Models;
using StudentManagement.Security;
using StudentManagement.Security.CustomTokenProvider;
using System;

namespace StudentManagement
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //注入HttpContextAccessor
            services.AddHttpContextAccessor();

            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"))
                );

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                //  options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;

                //通过自定义的CustomEmailConfirmation名称来覆盖旧有token名称，
                //是它与AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("ltmEmailConfirmation")
                //关联在一起
                options.Tokens.EmailConfirmationTokenProvider = "ltmEmailConfirmation";

                //指 在帐户被锁定之前允许的失败登录的次数。默认值为 5。
                options.Lockout.MaxFailedAccessAttempts = 5;
                //默认锁定时间为 15 分钟。
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            /// 修改所有令牌类型的有效时间为10个小时
            services.Configure<DataProtectionTokenProviderOptions>(
                opt =>
                {
                    opt.TokenLifespan = TimeSpan.FromHours(10);
                }
                );

            // 仅更改电子邮件验证令牌类型的有效时间为10秒
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(10);
            }
                );

            services.ConfigureApplicationCookie(options =>
            {
                //修改拒绝访问的路由地址
                options.AccessDeniedPath = new PathString("/Admin/AccessDenied");
                //修改登录地址的路由
                //   options.LoginPath = new PathString("/Admin/Login");
                //修改注销地址的路由
                //   options.LogoutPath = new PathString("/Admin/LogOut");
                //统一系统全局的Cookie名称
                options.Cookie.Name = "MockSchoolCookieName";
                // 登录用户Cookie的有效期
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //是否对Cookie启用滑动过期时间。
                options.SlidingExpiration = true;
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("ltmEmailConfirmation")

                ;

            // 策略结合声明授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));

                //策略结合多个角色进行授权
                options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("Admin", "User"));

                //  options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role","true"));

                //  options.AddPolicy("AllowedCountryPolicy", policy => policy.RequireClaim("Country","China","USA","UK"));

                //options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion( context => AuthorizeAccess(context)));

                options.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
                options.InvokeHandlersAfterFailure = false;
            });

            services.AddAuthentication().AddMicrosoftAccount(opt =>
            {
                opt.ClientId = _configuration["Authentication:Microsoft:ClientId"];
                opt.ClientSecret = _configuration["Authentication:Microsoft:ClientSecret"];
            }).AddGitHub(options =>
            {
                options.ClientId = _configuration["Authentication:Github:ClientId"];
                options.ClientSecret = _configuration["Authentication:Github:ClientSecret"];
            });

            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddScoped<IStudentRepository, SQLStudentRepository>();

            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            services.AddSingleton<DataProtectionPurposeStrings>();
        }

        // This method gets called by the runtim0e. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //如果环境是 Development，调用 Developer Exception Page
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");//拦截我们的异常
                app.UseStatusCodePagesWithReExecute("/Error/{0}"); //拦截404找不到的页面信息
            }

            app.UseStaticFiles();

            //身份认证中间件
            app.UseAuthentication();

            app.UseRouting();


            //身份认证(authentication)和授权(authorization)

            app.UseAuthorization();

           

            app.UseDataInitializer();

            // UseEndpoints 是一个可以处理跨不同中间件系统（如MVC、 Razor Pages、 Blazor、 SignalR和gRPC） 的路由系统。通过终结点路由可以使端点相互协作，并使系统比没有相互对话的终端中间件更全面。当然本书暂时不会涉及Razor Pages、 Blazor、 SignalR和gRPC，但是为了项目的长远规划,dotnet开发团队推荐使用终结点路由。

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute("default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //授权访问
        private bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            return context.User.IsInRole("Admin") &&
                    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                    context.User.IsInRole("Super Admin");
        }
    }
}