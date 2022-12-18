using ElectronicsShop.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ElectronicShope.DBModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicShope.DBModel.DBModel;
using ElectronicsShop.Services.Common.Mapper;
using ElectronicsShop.Core.Interfaces;
using ElectronicsShop.Services.AdminServices;
using ElectronicsShop.Repository.Interfaces;
using ElectronicsShop.Repository.Implementation;
using ElectronicShope.Mapper;

namespace ElectronicsShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<E_Shopping_DbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));


            //services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<E_Shopping_DbContext>();


            //services.AddIdentityCore<IdentityUser>()
            //      .AddRoles<IdentityRole>()
            //   .AddEntityFrameworkStores<E_Shopping_DbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
         .AddEntityFrameworkStores<E_Shopping_DbContext>();



            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(options =>
           {
               options.ExpireTimeSpan = TimeSpan.FromDays(1);
               options.SlidingExpiration = true;
               options.LoginPath = "/Account/Register";
           });

            BusinessAutoMapper.Configure();
            WebAutoMapper.Configure();

            services.AddMvc()
           .AddSessionStateTempDataProvider();
            services.AddSession();

            services.AddControllersWithViews();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
