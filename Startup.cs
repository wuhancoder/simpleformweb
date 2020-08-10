using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using simpleformweb.Services;

namespace simpleformweb
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
            services.AddControllersWithViews();
            services.AddScoped<IFormService, FormService>()
            .Configure<FormServiceOption>(Configuration.GetSection("FormService"));
            services.AddScoped<IFileService, FileService>()
            .Configure<FileServiceOption>(Configuration.GetSection("FileService"));
            services.AddApplicationInsightsTelemetry(Configuration["InstrumentKey"]);


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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id}");
                endpoints.MapControllerRoute(
                    name: "formJson",
                    pattern: "Form/{id}/config",
                    defaults: new { controller = "Form", action = "Config" }
                        );
                endpoints.MapControllerRoute(
                    name: "form",
                    pattern: "Form/{id}",
                    defaults: new { controller = "Form", action = "Index" }
                        );
                endpoints.MapControllerRoute(
            name: "formSubmit",
            pattern: "Form/{id}/submit",
            defaults: new { controller = "Form", action = "Submit" }
                );
            });
        }
    }
}
