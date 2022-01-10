using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SuperPanel.API.BackgroundTasks;
using SuperPanel.API.Infraestructure.Repositories;
using SuperPanel.API.NotificationsHub;
using SuperPanel.API.Service;
using System;

namespace SuperPanel.API
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

            services.AddControllers();
            services.AddInMemoryDbContext<SuperPanelContext>();
            services.AddAppModules(Configuration);
            services.AddHostedService<DeletionRequestServiceTask>();
            services.AddCors(options =>
                             {
                                 options.AddPolicy("CorsPolicy",
                                     builder => builder
                                     .AllowAnyMethod()
                                     .AllowAnyHeader()
                                     .SetIsOriginAllowed((host) => true)
                                     .AllowCredentials());
                             });
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SuperPanel.API", Version = "v1" });
            });
            services.AddMvc(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuperPanel.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}"
                    );
                endpoints.MapHub<NotificationHub>("/hub/notificationHub");
            });
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryDbContext<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
            => services.AddDbContext<TDbContext>(builder
                => builder.UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .ConfigureWarnings(w =>
                    {
                        w.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                    }
                    ), ServiceLifetime.Singleton);

        public static IServiceCollection AddAppModules(this IServiceCollection services, IConfiguration configuration)
        {
            //Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IDeletionRequestRepository, DeletionRequestRepository>();


            //Service
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<,>));
            services.AddScoped<IDeletionRequestService, DeletionRequestService>();


            return services;
        }
    }
}
