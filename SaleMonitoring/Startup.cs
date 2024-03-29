using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalaryCalc.Middlewares;
using SalaryCalc.Models;
using SalaryCalc.Models.Entities;
using SalaryCalc.Models.Repositories.EntityFramework;
using SalaryCalc.Models.Repositories.Interfaces;
using SalaryCalc.Service;
using System.Security.Claims;

namespace SalaryCalc
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        // Dependency Injection.
        public void ConfigureServices(IServiceCollection services)
        {
            // ���������� ������ �� appsettings.json.
            Configuration.Bind("Project", new Config());

            // ��������� ��������� MVC.
            services.AddControllersWithViews();

            // ���������� ���� �������.
            services.AddScoped<IUsersRepository, EFUsersRepository>();
            services.AddScoped<IPositionsRepository, EFPositionsRepository>();
            services.AddScoped<ISalariesRepository, EFSalariesRepository>();
            services.AddScoped<IProductsRepository, EFProductsRepository>();
            services.AddScoped<ICategoriesRepository, EFCategoriesRepository>();
            services.AddScoped<ISalesRepository, EFSalesRepository>();
            services.AddScoped<ISaleProductsRepository, EFSaleProductsRepository>();
            services.AddTransient<DataManager>();

            // ���������� �������� ��.
            services.AddDbContext<AppDbContext>(c => c.UseSqlServer(Config.ConnectionString));

            // ����������� identity � �������.
            services.AddIdentity<User, IdentityRole>(config =>
            {
                // ������������� ���������� � ������.
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>();  // ������������� ��� ���������, ������� ����� �����������
                                                        // � Identity ��� �������� ������. � �������� ���� ���������
                                                        // ����� ����������� ��.

            // ����������� authentication cookie.
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Authentication";  // ������� ��� cookie.
                config.Cookie.HttpOnly = true;  // Cookie ���������� �� ���������� �������.
                config.LoginPath = "/Account/Login";   // ���������� ������������ �� �������� ��������������.
            });

            // ��������� �����������.
            services.AddAuthorization(options =>
            {
                // ������� �������� ��� ����������� ��������.
                options.AddPolicy("Administrator", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "Administrator");
                });
                options.AddPolicy("User", builder =>
                {
                    builder.RequireAssertion(u => u.User.HasClaim(ClaimTypes.Role, "Administrator")
                                                  || u.User.HasClaim(ClaimTypes.Role, "User"));
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // � �������� ���������� ����� ������ ��������� ���������� �� �������.
           // if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseStatusCodePages();
            app.UseStaticFiles();   // ���������� ��������� ����������� ������ (css, js � ��.)

            app.UseRouting();   // ��������� ����������� �������������,
                                // ��������� ����� ���������� ����� ���������� ������� � ������������� ����������.

            // ���������� ���������� ������.
            app.UseMiddleware<ErrorHandlingMiddleware>();
            // ���������� ��������������.
            app.UseAuthentication();
            // ���������� �����������.
            app.UseAuthorization();

            // ������������� ������, ������� ����� ��������������.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
