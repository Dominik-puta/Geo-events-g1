using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoLocation.Repository;
using GeoLocation.Repository.Common;
using GeoLocation.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeoLocation.Web
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
            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped(typeof(IEventRepository), typeof(EventRepository));
            services.AddScoped(typeof(IEventCategoryRepository), typeof(EventCategoryRepository));
            services.AddScoped(typeof(IEventSubCategoryRepository), typeof(EventSubCategoryRepository));
            services.AddScoped(typeof(IVenueRepository), typeof(VenueRepository));
            services.AddScoped(typeof(IRsvpRepository), typeof(RsvpRepository));
            services.AddScoped(typeof(ICommentRepository), typeof(CommentRepository));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
