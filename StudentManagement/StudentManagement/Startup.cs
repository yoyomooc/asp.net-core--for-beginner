using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudentManagement.Middlewares;
using StudentManagement.Models;

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
        public void ConfigureServices(IServiceCollection services) {          


            services.AddDbContextPool<AppDbContext>(
                options=>options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"))                
                );


            services.Configure<IdentityOptions>(options=>
            {
                options.Password.RequiredLength = 6;
              //  options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;              

            });


            services.ConfigureApplicationCookie(options=>
            {
                options.AccessDeniedPath = "/Account/fangyu";
            });



            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>();


            services.AddMvc(config=>
            {

                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));


            }).AddXmlSerializerFormatters();

            services.AddScoped<IStudentRepository, SQLStudentRepository>();
        }



        // This method gets called by the runtim0e. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });



        }
    }
}
