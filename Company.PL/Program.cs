using Company.BLL.Profiles;
using Company.BLL.Services.AttachmentService;
using Company.BLL.Services.Classes;
using Company.BLL.Services.Interfaces;
using Company.DAL.Data.Contexts;
using Company.DAL.Models.IdentityModels;
using Company.DAL.Repositories.Classes;
using Company.DAL.Repositories.Interfaces;
using Company.PL.Settings;
using Company.PL.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.

            builder.Services.AddControllersWithViews(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            builder.Services.AddDbContext<AppDbContext>( options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
               .UseLazyLoadingProxies());

            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();

            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("TwilioSettings"));

            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ITwilioService, TwilioService>();

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddGoogle(o =>
            {
                o.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                o.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

            });

            #endregion
            var app = builder.Build();

            #region Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");
            #endregion

            app.Run();
        }
    }
}
