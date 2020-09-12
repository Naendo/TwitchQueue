using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TalkMaster.Services;
using TalkMaster.Services.Services.Provider;
using TalkMaster.View.Controllers.HttpController;
using TalkMaster.View.Models;
using TalkMaster.View.Models.Options;

namespace TalkMaster.View
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

            services.AddSignalR();

            services.AddSingleton<TwitchApiController>();
            services.AddSingleton<CookieController>();
            services.AddSingleton(new TokenOptions()
            {
                ClientId = Configuration["TokenOptions:ClientId"],
                ClientSecret = Configuration["TokenOptions:ClientSecret"],
                RedirectUri = Configuration["TokenOptions:RedirectUri"]
            });
            services.AddSingleton(new PaypalOptions()
            {
                ClientId = Configuration["PaypalOptions:ClientId"],
                ClientSecret = Configuration["PaypalOptions:ClientSecret"],
                RedirectUri = Configuration["PaypalOptions:RedirectUri"],
            });
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");

                endpoints.MapControllers();

                endpoints.MapHub<TwitchHub>("/queueHub");
            });
        }
    }
}
