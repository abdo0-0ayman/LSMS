using LSMS.data_access;
using LSMS.Models;
using LSMS.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace LSMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<ApplicationDbContext>(); // Add appropriate lifetime for ApplicationDbContext
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IScheduleGeneratorService, ScheduleGeneratorService>();
            builder.Services.AddScoped<IUpdateService, UpdateService>();



            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Home/Login"; // Set the login path
                options.SlidingExpiration = true;
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None; // Adjust SameSite policy as needed
				options.AccessDeniedPath = "/Home/Login";
			});

            builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("Students", policy => policy.RequireClaim(ClaimTypes.Role, "Students"));
				options.AddPolicy("Professors", policy => policy.RequireClaim(ClaimTypes.Role, "Professors"));
				options.AddPolicy("Admins", policy => policy.RequireClaim(ClaimTypes.Role, "Admins"));
			});

			var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication(); // Enable authentication middleware

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Logout}/{id?}");

            // seed DataBase
            AppDbInitializer.seed(app);

            app.Run();
        }
    }
}
