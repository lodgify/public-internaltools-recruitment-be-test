using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperPanel.App.Data;
using SuperPanel.App.Infrastructure;
using SuperPanel.App.Models;
using System;
using System.Linq;
using System.Text.Json;

namespace SuperPanel.App
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
            // Uncomment to generate new batch of bogus data
            // GenerateFakeData();

            services.AddControllersWithViews();
            services.AddOptions();
            services.Configure<DataOptions>(options => Configuration.GetSection("Data").Bind(options));

            // Data
            services.AddSingleton<IUserRepository, UserRepository>();
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
                app.UseExceptionHandler("/Users/Error");
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
                    pattern: "{controller=Users}/{action=Index}/{id?}");
            });
        }

        private void GenerateFakeData()
        {
            //Set the randomizer seed if you wish to generate repeatable data sets.
            Randomizer.Seed = new Random(8675309);

            var userIds = 10000;
            var faker = new Faker<User>()
                .CustomInstantiator(f => new User(userIds++))
                .RuleFor(u => u.Login, (f, u) => f.Internet.UserName())
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.CreatedAt, (f, u) => f.Date.Past(3));

            var users = faker.Generate(5000)
                    .OrderBy(_ => Randomizer.Seed.Next())
                    .ToList();

            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions() { WriteIndented = false });
            System.IO.File.WriteAllText("./../data/users.json", json);

        }
    }
}
