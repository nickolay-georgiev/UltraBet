namespace UltraBet.Web
{
    using System;
    using System.Reflection;

    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using UltraBet.Data;
    using UltraBet.Data.Common;
    using UltraBet.Data.Common.Repositories;
    using UltraBet.Data.Models;
    using UltraBet.Data.Repositories;
    using UltraBet.Data.Seeding;
    using UltraBet.Services;
    using UltraBet.Services.Data;
    using UltraBet.Services.Mapping;
    using UltraBet.Web.Hubs;
    using UltraBet.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddSignalR();

            services.AddHangfire(
                   config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                       .UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings()
                       .UseSqlServerStorage(
                           this.configuration.GetConnectionString("DefaultConnection"),
                           new SqlServerStorageOptions
                           {
                               CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                               SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                               QueuePollInterval = TimeSpan.Zero,
                               UseRecommendedIsolationLevel = true,
                               UsePageLocksOnDequeue = true,
                               DisableGlobalLocks = true,
                           }));

            services.AddHangfireServer();

            services.AddHttpClient();

            services.AddMemoryCache();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<ISportDataService, SportDataService>();
            services.AddTransient<IDeserializer, Deserializer>();
            services.AddTransient<ISportService, SportService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            AutoMapperConfig.RegisterMappings(typeof(MatchViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider)
                    .GetAwaiter()
                    .GetResult();

                this.SeedHangfireJobs(recurringJobManager);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 1 });
            app.UseHangfireDashboard(
             "/Hangfire",
             new DashboardOptions
             {
                 Authorization = new[] { new HangfireAuthorizationFilter() },
                 AppPath = "/",
             });

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<MatchHub>("/index");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }

        private void SeedHangfireJobs(IRecurringJobManager recurringJobManager)
        {
            //recurringJobManager.AddOrUpdate<FetchSportData>(nameof(FetchSportData), x => x.Work(), Cron.Minutely);
        }

        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                return true;
            }
        }
    }
}
