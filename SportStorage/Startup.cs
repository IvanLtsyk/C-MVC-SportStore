using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportStorage.Models;
using SportStorage.Models.ViewModels;

namespace SportStorage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<ApplicationDbContext>(builder => builder.UseNpgsql(
                    Configuration["Data:SportStoreProducts:ConnectionString"]))
                .AddDbContext<AppIdentityDbContext>(builder => builder.UseNpgsql(
                    Configuration["Data:SportStoreIdentity:ConnectionString"]))
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders()
                ;

            services
                .AddTransient<IProductRepository, EfProductRepository>()
                .AddTransient<IOrderRepository, EFOrderRepository>()
                .AddScoped<Cart>(sp => SessionCart.GetCart(sp))
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                ;

            services.AddMvc();
            services.AddSession();
            services.AddMemoryCache();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseStatusCodePages();

            app.UseSession();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{productPage:int}",
                    defaults: new {controller = "Product", action = "List"}
                );
                routes.MapRoute(
                    name: null,
                    template: "Page{productPage:int}",
                    defaults: new {controller = "Product", action = "List", productPage = 1}
                );
                routes.MapRoute(
                    name: null,
                    template: "{category}",
                    defaults: new {controller = "Product", action = "List", productPage = 1}
                );
                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new {controller = "Product", action = "List", productPage = 1}
                );
                routes.MapRoute(
                    name: null,
                    template: "{controller}/{action}/{id?}");
            });
            SeedData.EnsurePopulated(app);
            IdentitySeedData.EnsurePopulated(app);
        }
    }
}